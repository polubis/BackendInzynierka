using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.DbModels
{
    public class UserChangingEmail:Entity
    {
        [Required]
        public string Email { get; set; }


        public virtual User User { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
