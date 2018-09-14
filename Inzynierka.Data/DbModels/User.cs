using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.DbModels
{
    public class User : Entity
    {
        [Required]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }

        public DateTime? BirthDate { get; set; }
        public bool? Sex { get; set; }
        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public bool IsAcceptedRegister { get; set; } = false;

        [Required]
        public string CookiesActivateLink { get; set; }

        public virtual ICollection<Sound> Sounds { get; set; }

        public virtual ICollection<Quiz> Quizes { get; set; }
        public virtual Rate Rate { get; set; }
        public virtual UserSetting UserSetting { get; set; }
        public virtual UserChangingEmail UserChangingEmail { get; set; }
        public virtual ICollection<Motive> Motives { get; set; }

        public virtual ICollection<SharedMotives> SharedMotives { get; set; }
        // lista motyw

    }
}
