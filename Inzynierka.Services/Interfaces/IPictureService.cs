using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Inzynierka.Services.Interfaces
{
    public interface IPictureService
    {
        Task<string> SaveAvatar(int UserId, IFormFile image);
        Task<FileStream> ReturnPicture(int userId, string pictureType);
    }
}
