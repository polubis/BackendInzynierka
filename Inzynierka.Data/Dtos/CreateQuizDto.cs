using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class CreateQuizDto:BaseDto
    {
        public int PlaceInRank { get; set; }
        public int NumberOfPlayedGames { get; set; }
        public double Effectiveness { get; set; }
        public double ActualPoints { get; set; }
        public List<SimilarUserDto> SimilarUsers { get; set; }
        public double TimeAverage { get; set; }

        public int NumberOfAllPositiveAnswers { get; set; }
        public int NumberOfAllNegativeAnswers { get; set; }

    }
}
