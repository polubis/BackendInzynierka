using AutoMapper;
using Inzynierka.Data.DbModels;
using Inzynierka.Data.Dtos;
using Inzynierka.Data.HelpModels;
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
            CreateMap<RegisterViewModel, User>();
            CreateMap<User, LoginDto>();
            CreateMap<ActivateEmailDto, User>();
            CreateMap<SoundViewModel, Sound>();
            CreateMap<CreateQuizViewModel, Quiz>().ForMember(x => x.Questions, opt => opt.Ignore());
            CreateMap<RateDto, Rate>();
            CreateMap<QuizDto, Quiz>();
            CreateMap<QuestionViewModel, Question>();
            CreateMap<Question, QuestionDto>();
            CreateMap<ChangeUserSettingViewModel, UserSetting>();
            CreateMap<UserSetting, UserSettingsDto>();
            CreateMap<Motive, MotiveDto>();
            CreateMap<MotiveViewModel, Motive>();
            CreateMap<ChangeUserDataViewModel, User>().
                ForAllMembers(opts => opts.Condition((src, dest, srcUser) => srcUser != null));
            CreateMap<Motive, GetMotiveDto>();
            CreateMap<User, UserDto>();
        }
    }
}
