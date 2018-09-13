using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.HelpModels
{
    public class RateModel
    {
        public double RateValue { get; set; }
        public int CountOfQuizes { get; set; }
        public double PointsForAllGames { get; set; }

        public RateModel(double RateValue, int CountOfQuizes, double PointsForAllGames)
        {
            this.RateValue = RateValue;
            this.CountOfQuizes = CountOfQuizes;
            this.PointsForAllGames = PointsForAllGames;
        }
    }
}
