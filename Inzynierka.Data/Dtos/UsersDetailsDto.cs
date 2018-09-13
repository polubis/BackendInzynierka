using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class UsersDetailsDto:BaseDto
    {
        List<UserDetailsDto> Details { get; set; }
    }
}
