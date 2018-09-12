using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.CustomValidators
{
    public class MustBeCorrectCategoryName:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var givenCategoryName = value as string;
            if (givenCategoryName != "")
            {
                string[] CorrectCategoryNames = new string[] { "sound", "chord" };

                bool isCorrectSoundsNamesContainsValue = false;
                foreach (string element in CorrectCategoryNames)
                {
                    if (element == givenCategoryName)
                    {
                        isCorrectSoundsNamesContainsValue = true;
                        break;
                    }
                }

                return isCorrectSoundsNamesContainsValue;
            }
            return false;
        }
    }
}
