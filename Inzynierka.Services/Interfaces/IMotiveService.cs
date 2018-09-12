using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Inzynierka.Data.Dtos;
using Inzynierka.Data.HelpModels;

namespace Inzynierka.Services.Interfaces
{
    public interface IMotiveService
    {
        Task<ResultDto<MotiveDto>> AddMotive(MotiveViewModel viewModel, int userId);
        Task<ResultDto<MotiveDto>> EditMotive(MotiveViewModel viewModel, int motiveId, int UserId);
        Task<ResultDto<MotiveDto>> DeleteMotive(int motiveId, int UserId);
        Task<ResultDto<MotiveDto>> ShareMotive(int userToShareId, int userWhichSharingId, int motiveId);
    }
}
