using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.DbModels
{
    public class Rate:Entity
    {
        [Required]
        public virtual User User { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int NumberOfPlayedGames { get; set; }

        [Required]
        public double CurrentPercentageRate { get; set; }

        [Required]
        public double PointsForAllGames { get; set; }
    
    }
}
