using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Inzynierka.Data.CustomValidators;
using Inzynierka.Data.DbModels;
using Microsoft.AspNetCore.Http;

namespace Inzynierka.Data.ViewModels
{
    public class ChangeUserSettingViewModel
    {
        public int? MotiveId { get; set; }

        [CheckIsFileCorrectPicture ( ErrorMessage = "Pole zdjęcia musi zostać uzupełnione oraz posiadać odpowiedni format jpg/jpeg/png" )]
        public IFormFile Avatar { get; set; }
    }
}
