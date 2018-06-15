using Inzynierka.Data.Dtos;
using Inzynierka.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Services.Interfaces
{
    public interface IUserService
    {
        ResultDto<RegisterDto> Register(RegisterViewModel ViewModel);
    }
}
