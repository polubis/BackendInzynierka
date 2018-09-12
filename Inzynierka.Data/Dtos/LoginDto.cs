using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class LoginDto:BaseDto
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool Sex { get; set; }

        public UserSettingsDto UserSetting { get; set; }
        public List<MotiveDto> Motives { get; set; }
        
    }
}
