using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.CustomValidators
{
    public class MustHaveCorrectOctaveSymbol:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var givenOctaveSymbol = value as string;
            if (givenOctaveSymbol != "")
            {
                return givenOctaveSymbol == "1" || 
                    givenOctaveSymbol == "2" || 
                    givenOctaveSymbol == "3";   
            }
            return false;
        }
    }
}
