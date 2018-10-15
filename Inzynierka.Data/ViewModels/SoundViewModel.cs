using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Inzynierka.Data.CustomValidators;
using Microsoft.AspNetCore.Http;

namespace Inzynierka.Data.ViewModels
{
    public class SoundViewModel
    {
        [MustBeCorrectSoundName(ErrorMessage = "Nazwa oktawy nie należy do przedziału zadeklarowanych nazw")]
        public string Name { get; set; }

        [MustBeCorrectCategoryName(ErrorMessage = "Nazwa kategori powinna zgadzać się ze zdefiniowanymi nazwami")]
        public string Category { get; set; }

        [Required]
        public int GuitarString { get; set; }

        public int? SoundPosition { get; set; }

        [Required]
        public IFormFile Sound { get; set; }
    }
}
