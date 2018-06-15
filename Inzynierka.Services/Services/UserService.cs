using AutoMapper;
using Inzynierka.Data.DbModels;
using Inzynierka.Data.Dtos;
using Inzynierka.Data.ViewModels;
using Inzynierka.Repository.Interfaces;
using Inzynierka.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Services.Services
{
    public class UserService:IUserService
    {
        private readonly IRepository<User> _usersRepository;
        private readonly IMapper _mapper;
        public UserService(IRepository<User> usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
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

            user.PasswordHash = ViewModel.PasswordHash;

            if (_usersRepository.Insert(user) == 0)
            {
                result.Error = "Wystąpił błąd podczas zakładania konta";
                return result;

            }
            result.SuccessResult = _mapper.Map<RegisterDto>(user);

            return result;

        }
    }
}
