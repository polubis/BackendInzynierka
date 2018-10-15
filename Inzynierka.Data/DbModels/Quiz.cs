using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.DbModels
{
    public class Quiz:Entity
    {
        [Required]
        public string QuizType { get; set; }

        [Required]
        public double SecondsSpendOnQuiz { get; set; }

        [Required]
        public int NumberOfPositiveRates { get; set; }

        [Required]
        public int NumberOfNegativeRates { get; set; }

        [Required]
        public double RateInNumber { get; set; }

        [Required]
        public double PointsForGame { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public virtual ICollection<Question> Questions { get; set; }

        [Required]
        public int UserId { get; set; }

    }
}
