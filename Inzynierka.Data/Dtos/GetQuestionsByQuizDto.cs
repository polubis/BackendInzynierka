using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class GetQuestionsByQuizDto:BaseDto
    {
        public List<QuestionDto> Questions { get; set; }
    }
}
