using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class UserDetailsDto:BaseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool? Sex { get; set; }
        public List<QuizDto> Quizes { get; set; }
        public RateDto Rate { get; set; }
    }
}
