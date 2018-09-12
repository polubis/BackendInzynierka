using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.DbModels
{
    public class UserSetting:Entity
    {
        public virtual User User { get; set; }

        [Required]
        public int UserId { get; set; }

        public string PathToAvatar { get; set; }

        public virtual Motive Motive { get; set; }

        public int? MotiveId { get; set; }

     
    }
}
