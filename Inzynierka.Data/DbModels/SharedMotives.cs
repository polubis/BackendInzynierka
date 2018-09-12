using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.DbModels
{
    public class SharedMotives:Entity
    {
        [Required]
        public int MotiveId { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual Motive Motive { get; set; }
        public virtual User User { get; set; }
    }
}
