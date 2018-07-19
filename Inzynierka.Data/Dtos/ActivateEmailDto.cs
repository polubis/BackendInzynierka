using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class ActivateEmailDto:BaseDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
