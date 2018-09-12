using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Inzynierka.Data.DbModels;
using Inzynierka.Data.ViewModels;

namespace Inzynierka.Data.CustomValidators
{
    public class MustHaveCorrectQuestionsNumber:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var values = value as List<QuestionViewModel>;
            if (values != null)
            {
                int[] correctCounters = new int[] { 10, 20, 40 };

                int valuesCount = values.Count;

                bool hasValuesCorrectCount = false;

                foreach(int counter in correctCounters)
                {
                    if (counter == valuesCount)
                        hasValuesCorrectCount = true;
                }

                return hasValuesCorrectCount;
            }
            return false;
        }
    }
}
