using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.DbModels
{
    public class Motive:Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string MainColor { get; set; }

        [Required]
        public string FontColor { get; set; }

        public virtual User User { get; set; }

        [Required]
        public int UserId { get; set; }

        public UserSetting UserSetting { get; set; }
        public virtual ICollection<SharedMotives> SharedMotives { get; set; }

    }
}
