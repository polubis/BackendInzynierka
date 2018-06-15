using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Inzynierka.Data.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "Nazwa użytkownika powinna mieć od 5 do 30 znaków", MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [StringLength(30, ErrorMessage = "Długość imienia powinna mieć od 3 do 30 znaków", MinimumLength = 3)]
        public string FirstName { get; set; }

        [StringLength(30, ErrorMessage = "Długość nazwiska powinna mieć od 3 do 30 znaków", MinimumLength = 3)]
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }
        public bool Sex { get; set; }
    }
}
