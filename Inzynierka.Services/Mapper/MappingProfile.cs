using AutoMapper;
using Inzynierka.Data.DbModels;
using Inzynierka.Data.Dtos;
using Inzynierka.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Services.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<RegisterViewModel, User>();
            CreateMap<User, LoginDto>();
        }
    }
}
