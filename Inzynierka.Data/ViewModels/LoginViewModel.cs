using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_-]*$", ErrorMessage = "Nazwa użytkownia może składać się z liter, cyfr, '_', '_'")]
        [StringLength(25, ErrorMessage = "Nazwa użytkownika powinna mieć od 5 do 25 znaków", MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Hasło powinno mieć od 5 do 20 znaków", MinimumLength = 5)]
        public string Password { get; set; }

    }
}
