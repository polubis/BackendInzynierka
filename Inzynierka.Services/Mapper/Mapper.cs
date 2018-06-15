using AutoMapper;
using Inzynierka.Data.DbModels;
using Inzynierka.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Services.Mapper
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<User, UserDto>();
        }
    }
}
