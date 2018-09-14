using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.ViewModels
{
    public class ChangeEmailViewModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "Adres email powinien mieć od 7 do 30 znaków", MinimumLength = 7)]
        [DataType(DataType.EmailAddress)]
        public string OldEmailAdress { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Adres email powinien mieć od 7 do 30 znaków", MinimumLength = 7)]
        [DataType(DataType.EmailAddress)]
        public string NewEmailAdress { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Hasło powinno mieć od 5 do 20 znaków", MinimumLength = 5)]
        public string CurrentPassword { get; set; }

    }
}
