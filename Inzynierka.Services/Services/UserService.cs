using AutoMapper;
using Inzynierka.Data.DbModels;
using Inzynierka.Data.Dtos;
using Inzynierka.Data.ViewModels;
using Inzynierka.Repository.Interfaces;
using Inzynierka.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Inzynierka.Data.HelpModels;

namespace Inzynierka.Services.Services
{
    public class UserService:IUserService
    {
        private readonly IRepository<User> _usersRepository;
        private readonly IMapper _mapper;
        private readonly IConfigurationManager _configurationManager;
        private readonly IEmailService _emailService;
        private readonly IPictureService _pictureService;
        private readonly IRepository<Motive> _motiveRepository;
        private readonly IRepository<UserSetting> _userSettingsRepository;
        private readonly IRepository<SharedMotives> _sharedMotivesRepository;
        private readonly EmailConfig EmailConfig = new EmailConfig();
        private readonly IRepository<UserChangingEmail> _usersChangingEmailRepository;
        public UserService(IRepository<Motive> motiveRepository,
            IRepository<User> usersRepository, IPictureService pictureService, 
            IRepository<UserSetting> userSettingsRepository, 
            IRepository<SharedMotives> sharedMotivesRepository,
            IRepository<UserChangingEmail> usersChangingEmailRepository,
            IMapper mapper, IConfigurationManager configurationManager, IEmailService emailService)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _configurationManager = configurationManager;
            _emailService = emailService;
            _userSettingsRepository = userSettingsRepository;
            _pictureService = pictureService;
            _motiveRepository = motiveRepository;
            _usersChangingEmailRepository = usersChangingEmailRepository;
            _sharedMotivesRepository = sharedMotivesRepository;

        }
        private string GetHash(string text)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public async Task<ResultDto<UsersDetailsDto>> GetUsers(int limit, int page, string search, int userId)
        {
            var result = new ResultDto<UsersDetailsDto>();

            IEnumerable<User> users;

            if(search == "")
            {
                users = await Task.Run(() => _usersRepository.GetAllByWithLimit(x => x.Id != userId, x => x.Username,
                null, limit, page, x => x.Rate, x => x.Quizes));
            }
            else
            {
                users = await Task.Run(() => _usersRepository.GetAllByWithLimit(x => x.Id != userId &&
                    x.Username.Contains(search), x => x.Username,
                    null, limit, page, x => x.Rate, x => x.Quizes));
            }

            if(users.Count() == 0)
            {
                result.Errors.Add("Brak wyników dla podanych parametrów");
                return result;
            }

            var userDetailsDto = new UsersDetailsDto();
            var mappedDetails = _mapper.Map<List<User>, List<UserDetailsDto>>(users.ToList());


            bool? IsShared = true;

            for(int i = 0; i < mappedDetails.Count; i++)
            {
                var motives = await Task.Run(() => _motiveRepository.GetAllBy(x => x.IsSharedGlobally == IsShared && x.UserId ==
                    mappedDetails[i].Rate.User.Id).ToList());

                mappedDetails[i].Motives = _mapper.Map<List<Motive>, List<MotiveDto>>(motives);

                var sharedMotives = await Task.Run(() => _sharedMotivesRepository.GetAllBy(x => x.UserId == userId, x => x.Motive).ToList());

                var listOfSharedMotives = new List<MotiveDto>();

                for(int j = 0; j < sharedMotives.Count(); j++)
                {
                    var mappedSharedMotive = _mapper.Map<Motive, MotiveDto>(sharedMotives.ElementAt(j).Motive);
                    listOfSharedMotives.Add(mappedSharedMotive);
                }

                mappedDetails[i].SharedMotivesForLoggedUser = listOfSharedMotives;
            }

            userDetailsDto.Details = mappedDetails;
            result.SuccessResult = userDetailsDto;

            return result;
        }
        private string GetToken(User user, string secretKey, string issuer, DateTime? expirationDate = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
            };
            var token = new JwtSecurityToken(issuer, issuer, claims, expires: expirationDate, signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateActivationLink(User user)
        {
            return GetHash(user.Username + user.Email);
        }

        public async Task<ResultDto<LoginDto>> GetUserData(int userId)
        {
            var result = new ResultDto<LoginDto>();

            var user = await Task.Run(() => _usersRepository.GetBy(x => x.Id == userId, x => x.UserSetting, x => x.Motives));

            var getUserResult = _mapper.Map<LoginDto>(user);

            var actualMotive = _userSettingsRepository.GetBy(x => x.UserId == user.Id, x => x.Motive);

            if (actualMotive != null)
            {
                getUserResult.UserSetting.MotiveDto = _mapper.Map<MotiveDto>(actualMotive.Motive);
            }

            result.SuccessResult = getUserResult;

            return result;
        }

        public async Task<ResultDto<LoginDto>> Login(LoginViewModel loginModel)
        {
            var result = new ResultDto<LoginDto>();
            var user =  await Task.Run(() => _usersRepository.GetBy(x => x.Username == loginModel.Username, x => x.UserSetting, x => x.Motives));

            if (user == null || GetHash(loginModel.Password) != user.PasswordHash)
            {
                result.Errors.Add("Błędny login lub hasło");
                return result;
            }

            if (!user.IsAcceptedRegister)
            {
                result.Errors.Add("To konto nie zostało jeszcze aktywowane");
                return result;
            }

            var loginDto = _mapper.Map<LoginDto>(user);

            var actualMotive = _userSettingsRepository.GetBy(x => x.UserId == user.Id, x => x.Motive);

            if(actualMotive != null)
            {
                loginDto.UserSetting.MotiveDto = _mapper.Map<MotiveDto>(actualMotive.Motive);

            }

            loginDto.Token = GetToken(user, "theKeyGeneratedToken",
                "http://localhost:52535", DateTime.Now.AddDays(7));

            result.SuccessResult = loginDto;
      
            return result;
        }

        public async Task<ResultDto<ChangeEmailDto>> ChangeEmail(ChangeEmailViewModel viewModel, int userId)
        {
            var result = new ResultDto<ChangeEmailDto>();

            var user = _usersRepository.GetBy(x => x.Id == userId);

            if(user == null)
            {
                result.Errors.Add("Użytkownik o podanych parametrach nie istnieje");
                return result;
            }

            if(user.PasswordHash != GetHash(viewModel.CurrentPassword) || 
                user.Email != viewModel.OldEmailAdress)
            {
                result.Errors.Add("Wprowadzono nieprawidłowe dane dotyczące zmiany adresu email");
                return result;
            }

            bool isUserWithGivenEmailExist = _usersRepository.Exist(x => x.Email == viewModel.NewEmailAdress);

            if (isUserWithGivenEmailExist)
            {
                result.Errors.Add("Użytkownik o podanym adresie mailowym już istnieje");
                return result;
            }

            var userChangingEmail = _usersChangingEmailRepository.GetBy(x => x.UserId == userId);
            var userChangingEmailModel = new UserChangingEmail();
            userChangingEmailModel.UserId = userId;
            userChangingEmailModel.Email = viewModel.NewEmailAdress;

            if (userChangingEmail == null)
            {
                int isInsertedChangingEmail = await _usersChangingEmailRepository.Insert(userChangingEmailModel);
                if (isInsertedChangingEmail == 0)
                {
                    result.Errors.Add("Wystąpił problem podczas zmiany adresu mailowego");
                    return result;
                }
            }
            else
            {
                int isUpdatedChangingEmail = _usersChangingEmailRepository.Update(userChangingEmailModel);
                if (isUpdatedChangingEmail == 0)
                {
                    result.Errors.Add("Wystąpił problem podczas zmiany adresu mailowego");
                    return result;
                }
            }

            var helpUserObject = new User();
            helpUserObject.Email = viewModel.NewEmailAdress;
            helpUserObject.Username = user.Username;

            user.CookiesActivateLink = GenerateActivationLink(helpUserObject);

            int isUpdated = await Task.Run(() => _usersRepository.Update(user));

            if(isUpdated == 0)
            {
                result.Errors.Add("Wystąpił błąd podczas zmiany adresu email");
                return result;
            }

            try
            {
                await _emailService.SendEmailAfterRegister(viewModel.OldEmailAdress, user.CookiesActivateLink,
                      "Zmiana adresu mailowego", user.Username, EmailConfig.changeEmailMessage, EmailConfig.changeEmailLink);
            }
            catch
            {
                result.Errors.Add("Wystapił błąd podczas zmiany adresu mailowego");
            }

            return result;
        }
        public async Task<ResultDto<RegisterDto>> Register(RegisterViewModel ViewModel)
        {
            var result = new ResultDto<RegisterDto>();

            if(_usersRepository.Exist(x => x.Username == ViewModel.Username))
            {
                result.Errors.Add("Taki użytkownik już istnieje");
                return result;
            }

            if(_usersRepository.Exist(x => x.Email == ViewModel.Email))
            {
                result.Errors.Add("Użytkownik o podanym adresie już istnieje");
                return result;
            }

            var user = _mapper.Map<User>(ViewModel);

            user.PasswordHash = GetHash(ViewModel.Password);
            user.CookiesActivateLink = GenerateActivationLink(user);

            int isInserted = await _usersRepository.Insert(user);

            if (isInserted == 0)
            {
                result.Errors.Add("Wystąpił błąd podczas zakładania konta");
                return result;

            }

            await _emailService.SendEmailAfterRegister(ViewModel.Email, user.CookiesActivateLink, 
                "Potwierdzenie rejestracji", ViewModel.Username, EmailConfig.registerMessage, EmailConfig.registerLink);

            return result;
        }

        public async Task<ResultDto<ActivateEmailDto>> ConfirmChangeEmailLink(string link)
        {
            var result = new ResultDto<ActivateEmailDto>();

            var user = await Task.Run(() => _usersRepository.GetBy(x => x.CookiesActivateLink == link));

            if(user == null)
            {
                result.Errors.Add("Nieprawidłowy link aktywacyjny");
                return result;
            }

            var userChangingEmail = await Task.Run(() => _usersChangingEmailRepository.GetBy(x => x.UserId == user.Id));

            if(userChangingEmail == null)
            {
                result.Errors.Add("Procedura zmiany adresu email nie jest aktywna");
                return result;
            }

            user.Email = userChangingEmail.Email;

            int isUserUpdated = _usersRepository.Update(user);

            if(isUserUpdated == 0)
            {
                result.Errors.Add("Wystąpił błąd podczas aktywowania nowego adresu email");
                return result;
            }

            var activateEmailDto = _mapper.Map<User, ActivateEmailDto>(user);
            result.SuccessResult = activateEmailDto;

            return result;
        }


        public async Task<ResultDto<ActivateEmailDto>> ConfirmActivationLink(string link)
        {
            var result = new ResultDto<ActivateEmailDto>();

            var user = await Task.Run(() =>
                _usersRepository.GetBy(x => x.CookiesActivateLink == link)
            );

            if (user == null)
            {
                result.Errors.Add("Nieprawidłowy link aktywacyjny");
                return result;
            }

            if (user.IsAcceptedRegister)
            {
                result.Errors.Add("Rejestracja już została potwierdzona");
                return result;
            }

            user.CookiesActivateLink = link;
            user.IsAcceptedRegister = true;

            if (_usersRepository.Update(user) == 0)
            {
                result.Errors.Add("Wystąpił błąd podczas procesu aktywacji konta");
                return result;
            }

            var updatedUser = await Task.Run(() => _usersRepository.GetBy(x => x.CookiesActivateLink == link));

            result.SuccessResult = _mapper.Map<ActivateEmailDto>(updatedUser);

            return result;
        }

        public async Task<ResultDto<ReturnUserSettingsDto>> CreateSetting(ChangeUserSettingViewModel viewModel, int UserId)
        {
            var result = new ResultDto<ReturnUserSettingsDto>();

            if (viewModel.MotiveId == null && viewModel.Avatar == null)
            {
                result.Errors.Add("Nie podano wymaganych wartości do utworzenia ustawienia");

                return result;
            }

            bool isUserSettingAlreadyCreated = _userSettingsRepository.Exist(x => x.UserId == UserId);

            if(isUserSettingAlreadyCreated)
            {
                result.Errors.Add("Ustawienia dla tego konta zostały już utworzone");

                return result;
            }

            if(viewModel.MotiveId != null)
            {
                var isMotiveExist = _motiveRepository.Exist(x => x.Id == viewModel.MotiveId && x.UserId == UserId);

                if (!isMotiveExist)
                {
                    result.Errors.Add("Motyw o podanych parametrach nie istnieje");

                    return result;
                }
            }

            var setting = _mapper.Map<ChangeUserSettingViewModel, UserSetting>(viewModel);
            setting.UserId = UserId;

            if(viewModel.Avatar != null)
                setting.PathToAvatar = await _pictureService.SaveAvatar(UserId, viewModel.Avatar);

            if(setting.PathToAvatar == null)
            {
                result.Errors.Add("Wystąpił błąd podczas dodawania zdjęcia");

                return result;
            }

            int isInsertedCorrectly = await _userSettingsRepository.Insert(setting);

            if (isInsertedCorrectly == 0)
            {
                result.Errors.Add("Wystąpił błąd podczas dodawania ustawień");

                return result;
            }

            return result;
        }

        public async Task<ResultDto<ReturnUserSettingsDto>> ChangeSetting(ChangeUserSettingViewModel viewModel, int UserId)
        {
            var result = new ResultDto<ReturnUserSettingsDto>();

            if (viewModel.MotiveId == null && viewModel.Avatar == null)
            {
                result.Errors.Add("Nie podano wymaganych wartości do utworzenia ustawienia");

                return result;
            }

            var userSetting = _userSettingsRepository.GetBy(x => x.UserId == UserId);

            if (userSetting == null)
            {
                result.Errors.Add("Brak stworzonego ustawienia dla tego konta. Najpierw utwórz nowe");

                return result;
            }

            if(viewModel.MotiveId != null)
            {
                bool isMotiveExist = _motiveRepository.Exist(x => x.Id == viewModel.MotiveId);

                if (!isMotiveExist)
                {
                    result.Errors.Add("Motyw o podanych parametrach nie istnieje");

                    return result;
                }
            }

            if (userSetting.MotiveId != viewModel.MotiveId)
                userSetting.MotiveId = viewModel.MotiveId;

            if(viewModel.Avatar != null)
                userSetting.PathToAvatar = await _pictureService.SaveAvatar(UserId, viewModel.Avatar);

            if (userSetting.PathToAvatar == null)
            {
                result.Errors.Add("Wystąpił błąd podczas dodawania zdjęcia profilowego");

                return result;
            }

            int isUpdated = await Task.Run(() =>
             _userSettingsRepository.Update(userSetting)
            );

            if (isUpdated == 0)
            {
                result.Errors.Add("Wystąpił błąd podczas aktualizacji ustawień");

                return result;
            }

            return result;
        }

        public async Task<ResultDto<EmptyDto>> ChangeUserData(ChangeUserDataViewModel viewModel, int UserId)
        {
            var result = new ResultDto<EmptyDto>();

            var user = _usersRepository.GetBy(x => x.Id == UserId);

            if(viewModel.Username != null)
            {
                bool isOtherUserHaveTheSameUsername = _usersRepository.Exist(x => x.Username == viewModel.Username);
                if (isOtherUserHaveTheSameUsername)
                {
                    result.Errors.Add("Ta nazwa użytkownika jest już zajęta");
                    return result;
                }
            }

            var newUser = _mapper.Map(viewModel, user);

            if (viewModel.NewPassword != null && viewModel.OldPassword != null)
            {
                if(user.PasswordHash != GetHash(viewModel.OldPassword))
                {
                    result.Errors.Add("Stare hasło jest nieprawidłowe");
                    return result;
                }

                if(user.PasswordHash == GetHash(viewModel.NewPassword))
                {
                    result.Errors.Add("Nowe hasło jest takie same jak stare");
                    return result;
                }


                newUser.PasswordHash = GetHash(viewModel.NewPassword);
            }

            int isUpdated = await Task.Run(() => _usersRepository.Update(newUser));

            if(isUpdated == 0)
                result.Errors.Add("Wystapił błąd podczas zapisywania zmian");

            return result;
        }
    }
}
