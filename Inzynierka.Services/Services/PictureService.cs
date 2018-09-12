using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Inzynierka.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace Inzynierka.Services.Services
{
    public class PictureService:IPictureService
    {

        private readonly string NameOfFolderToSaveAllAvatars = "avatars";
        private readonly string NameOfFolderForAllImages = "pictures";
   
        public async Task<string> SaveAvatar(int userId, IFormFile file)
        {
            string pathToPictures = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", NameOfFolderForAllImages);
            string pathToAvatars = Path.Combine(pathToPictures, NameOfFolderToSaveAllAvatars);

            bool wasFoldersCreatedCorrectly = await Task.Run(() => CreateDirectoryStructure(pathToPictures, pathToAvatars));

            if (!wasFoldersCreatedCorrectly)
                return null;

            string fileNameWithoutDashes = Regex.Replace(file.FileName, "[_]", string.Empty);

            string pathToSaveImage = Path.Combine(pathToAvatars, userId.ToString() + "_" + fileNameWithoutDashes);

            try
            {
                if (File.Exists(pathToSaveImage))
                {
                    File.Delete(pathToSaveImage);
                }
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                   // var bytedFile = memoryStream.ToArray();

                    using (FileStream fileStream = new FileStream(pathToSaveImage, FileMode.Create, FileAccess.Write))
                    {
                        memoryStream.WriteTo(fileStream);
                    }
                }
            }
            catch
            {
                return null;
            }
           
            return pathToSaveImage;
        }
        private bool CreateDirectoryStructure(string pathToPictures, string pathToFolderBasedOnImgType)
        {
            try
            {

                if (!Directory.Exists(pathToPictures))
                {
                    Directory.CreateDirectory(pathToPictures);
                }

                if (!Directory.Exists(pathToFolderBasedOnImgType))
                {
                    Directory.CreateDirectory(pathToFolderBasedOnImgType);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<FileStream> ReturnPicture(int userId, string pictureType)
        {
            string pathToGetPicture = "";

            if(pictureType == NameOfFolderToSaveAllAvatars)
            {
                pathToGetPicture = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", 
                    NameOfFolderForAllImages, NameOfFolderToSaveAllAvatars);
            }

            return await Task.Run(() =>
                File.OpenRead(pathToGetPicture)
            ); 
        }
      
    }
}
