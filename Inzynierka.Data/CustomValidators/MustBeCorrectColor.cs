using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Inzynierka.Data.CustomValidators
{
    public class MustBeCorrectColor:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var givenColor = value as string;
            if (givenColor != "")
            {

                if (givenColor[0] != '#')
                    return false;

                if (givenColor.Length != 7)
                    return false;

                Regex regex = new Regex(@"^[A-Z0-9#]+$");

                if (!regex.IsMatch(givenColor))
                    return false;

                return true;
            }
            return false;
        }
    }
}
