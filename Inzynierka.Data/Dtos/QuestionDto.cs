using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class QuestionDto
    {
        public string CorrectAnswer { get; set; }
        public string Answer { get; set; }
        public bool WasAnswerCorrect { get; set; }
        public int TimeForAnswerInSeconds { get; set; }
    }
}
