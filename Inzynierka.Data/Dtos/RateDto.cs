using System;
using System.Collections.Generic;
using System.Text;
using Inzynierka.Data.DbModels;

namespace Inzynierka.Data.Dtos
{
    public class RateDto:BaseDto
    {
        public UserDto User { get; set; }
        public int NumberOfPlayedGames { get; set; }

        public double CurrentPercentageRate { get; set; }
        public double PointsForAllGames { get; set; }
    }
}
