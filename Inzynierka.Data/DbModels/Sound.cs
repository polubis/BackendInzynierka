using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.DbModels
{
    public class Sound:Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public virtual User User { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public int GuitarString { get; set; }

        public int? SoundPosition { get; set; }

        public string ChordType { get; set; }

    }
}
