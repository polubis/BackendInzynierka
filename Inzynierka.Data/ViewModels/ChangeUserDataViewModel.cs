using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.ViewModels
{
    public class ChangeUserDataViewModel
    {

        [StringLength(30, ErrorMessage = "Nazwa użytkownika powinna mieć od 5 do 30 znaków", MinimumLength = 5)]
        public string Username { get; set; }

        [StringLength(30, ErrorMessage = "Adres email powinien mieć od 7 do 30 znaków", MinimumLength = 7)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "Hasło powinno mieć od 5 do 20 znaków", MinimumLength = 5)]
        public string NewPassword { get; set; }

        [StringLength(20, ErrorMessage = "Hasło powinno mieć od 5 do 20 znaków", MinimumLength = 5)]
        public string OldPassword { get; set; }

        [StringLength(30, ErrorMessage = "Długość imienia powinna mieć od 3 do 30 znaków", MinimumLength = 3)]
        public string FirstName { get; set; }

        [StringLength(30, ErrorMessage = "Długość nazwiska powinna mieć od 3 do 30 znaków", MinimumLength = 3)]
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }
        public bool? Sex { get; set; }
    }
}
