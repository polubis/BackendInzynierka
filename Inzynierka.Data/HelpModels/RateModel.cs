using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.HelpModels
{
    public class RateModel
    {
        public double RateValue { get; set; }
        public int CountOfQuizes { get; set; }

        public RateModel(double RateValue, int CountOfQuizes)
        {
            this.RateValue = RateValue;
            this.CountOfQuizes = CountOfQuizes;
        }
    }
}
