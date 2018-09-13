using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Inzynierka.Data.CustomValidators;

namespace Inzynierka.Data.HelpModels
{
    public class MotiveViewModel
    {
        [Required]
        public string Name { get; set; }

        [MustBeCorrectColor(ErrorMessage = "Wartość dla pola głównego koloru jest niezgodne ze zdefiniowanym standardem")]
        public string MainColor { get; set; }

        [MustBeCorrectColor(ErrorMessage = "Wartość dla pola koloru tekstu jest niezgodna ze zdefiniowanym standardem")]
        public string FontColor { get; set; }
    }
}
