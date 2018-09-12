using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class RateDto:BaseDto
    {

        public int NumberOfPlayedGames { get; set; }

        public double CurrentPercentageRate { get; set; }
    }
}
