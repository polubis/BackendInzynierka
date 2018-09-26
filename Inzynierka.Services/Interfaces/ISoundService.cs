using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Inzynierka.Data.Dtos;
using Inzynierka.Data.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Inzynierka.Services.Interfaces
{
    public interface ISoundService
    {
        Task<ResultDto<UploadSoundDto>> UploadSounds(SoundViewModel soundModel, int userId);
        Task<ResultDto<GetZippedSoundsDto>> DownloadZippedSoundsMixed();
        Task<ResultDto<GetZippedSoundsDto>> DownloadZippedIntervalsByType(string type, int numberOfIntervalsToCreate);
        Task<ResultDto<GetSoundsByCategoryDto>> GetSoundNamesByCategory(string category);
    }
}
