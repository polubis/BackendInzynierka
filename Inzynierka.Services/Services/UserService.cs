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

namespace Inzynierka.Services.Services
{
    public class UserService:IUserService
    {
        private readonly IRepository<User> _usersRepository;
        private readonly IMapper _mapper;
        private readonly IConfigurationManager _configurationManager;
        private readonly IEmailService _emailService;
        public UserService(IRepository<User> usersRepository, IMapper mapper, IConfigurationManager configurationManager, IEmailService emailService)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _configurationManager = configurationManager;
            _emailService = emailService;

        }
        private string GetHash(string text)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
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
        public ResultDto<LoginDto> Login(LoginViewModel loginModel)
        {
            var result = new ResultDto<LoginDto>();
            var user = _usersRepository.GetBy(x => x.Username == loginModel.Username);

            if (user == null || GetHash(loginModel.Password) != user.PasswordHash)
            {
                result.Error = "Błędny login lub hasło";
                return result;
            }
            var loginDto = _mapper.Map<LoginDto>(user);

            loginDto.Token = GetToken(user, "theKeyGeneratedToken",
                "http://localhost:52535", DateTime.Now.AddDays(7));

            result.SuccessResult = loginDto;
      
            return result;
        }

        public ResultDto<RegisterDto> Register(RegisterViewModel ViewModel)
        {
            var result = new ResultDto<RegisterDto>();

            if(_usersRepository.Exist(x => x.Username == ViewModel.Username))
            {
                result.Error = "Taki użytkownik już istnieje";
                return result;
            }

            if(_usersRepository.Exist(x => x.Email == ViewModel.Email))
            {
                result.Error = "Użytkownik o podanym adresie już istnieje";
                return result;
            }

            var user = _mapper.Map<User>(ViewModel);

            user.PasswordHash = GetHash(ViewModel.Password);
            user.CookiesActivateLink = GenerateActivationLink(user);

            if (_usersRepository.Insert(user) == 0)
            {
                result.Error = "Wystąpił błąd podczas zakładania konta";
                return result;

            }


            result.SuccessResult = _mapper.Map<RegisterDto>(user);

            // Tu chcialbym asynchronicznie wyslac wiadomosc
            _emailService.SendEmailAfterRegister(ViewModel.Email, user.CookiesActivateLink, "Potwierdzenie rejestracji", ViewModel.Username);

            return result;
        }

        

        public ResultDto<ActivateEmailDto> ConfirmRegister(string link)
        {
            var result = new ResultDto<ActivateEmailDto>();

            var user = _usersRepository.GetBy(x => x.CookiesActivateLink == link);

            if (user == null)
            {
                result.Error = "Nie prawidłowy link aktywacyjny";
                return result;
            }

            if (user.isAcceptedRegister)
            {
                result.Error = "Te konto już zostało aktywowane";
                return result;
            }

            user.CookiesActivateLink = link;
            user.isAcceptedRegister = true;

            if (_usersRepository.Update(user) == 0)
            {
                result.Error = "Wystąpił błąd podczas procesu aktywacji konta";
                return result;
            }

            var updatedUser = _usersRepository.GetBy(x => x.CookiesActivateLink == link);

            result.SuccessResult = _mapper.Map<ActivateEmailDto>(updatedUser);

            return result;
        }
       
    }
}
