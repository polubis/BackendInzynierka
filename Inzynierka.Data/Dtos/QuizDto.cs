using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class QuizDto:BaseDto
    {
        public int Id { get; set; }
        public string QuizType { get; set; }
        public int SecondsSpendOnQuiz { get; set; }

        public int NumberOfPositiveRates { get; set; }

        public int NumberOfNegativeRates { get; set; }

        public double RateInNumber { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
