using Inzynierka.Data.Dtos;
using Inzynierka.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inzynierka.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResultDto<RegisterDto>> Register(RegisterViewModel registerModel);
        Task<ResultDto<LoginDto>> Login(LoginViewModel loginModel);

        Task<ResultDto<ActivateEmailDto>> ConfirmRegister(string link);
        Task<ResultDto<ReturnUserSettingsDto>> ChangeSetting(ChangeUserSettingViewModel viewModel, int UserId);

        Task<ResultDto<ReturnUserSettingsDto>> CreateSetting(ChangeUserSettingViewModel viewModel, int UserId);

        Task<ResultDto<EmptyDto>> ChangeUserData(ChangeUserDataViewModel viewModel, int userId);
    }
}
