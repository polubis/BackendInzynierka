using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class UserDetailsDto:BaseDto
    {
        public DateTime CreationDate { get; set; }
        public List<QuizDto> Quizes { get; set; }
        public RateDto Rate { get; set; }
        public List<MotiveDto> Motives { get; set; }

        public List<MotiveDto> SharedMotivesForLoggedUser { get; set; }
    }
}
