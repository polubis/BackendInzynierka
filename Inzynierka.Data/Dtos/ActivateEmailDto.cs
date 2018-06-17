using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class ActivateEmailDto:BaseDto
    {
        public string Username { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
