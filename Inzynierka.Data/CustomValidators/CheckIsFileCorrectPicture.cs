using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Inzynierka.Data.CustomValidators
{
    public class CheckIsFileCorrectPicture:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var givenFile = value as IFormFile;

            if(givenFile != null && givenFile.ContentType != "image/jpeg" && 
                 givenFile.ContentType != "image/png" && givenFile.ContentType != "image/jpg")
            {
                return false;
            }

            return true;
        }
    }
}
