using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.HelpModels
{
    public class RateModel
    {
        public double PercentageRate { get; set; }
        public int CountOfQuizes { get; set; }
        public double PointsForAllGames { get; set; }
        public int NumberOfAllPositiveAnswers { get; set; }
        public int NumberOfAllNegativeAnswers { get; set; }


        public RateModel(double PercentageRate, int CountOfQuizes, double PointsForAllGames, int NumberOfAllPositiveAnswers, int NumberOfAllNegativeAnswers)
        {
            this.PercentageRate = PercentageRate;
            this.CountOfQuizes = CountOfQuizes;
            this.PointsForAllGames = PointsForAllGames;
            this.NumberOfAllPositiveAnswers = NumberOfAllPositiveAnswers;
            this.NumberOfAllNegativeAnswers = NumberOfAllNegativeAnswers;
        }
    }
}
