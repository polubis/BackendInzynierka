using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.CustomValidators
{
    public class MustBeCorrectSoundName:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var givenSoundsName = value as string;
            if (givenSoundsName != "")
            {
                string[] CorrectSoundNames = new string[] { "A", "Ais", "H", "C", "Cis", "D", "Dis", "E", "F", "Fis", "G", "Gis" };

                bool isCorrectSoundsNamesContainsValue = false;
                foreach(string element in CorrectSoundNames)
                {
                    if(element == givenSoundsName)
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
