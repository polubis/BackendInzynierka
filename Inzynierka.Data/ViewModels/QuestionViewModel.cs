using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.ViewModels
{
    public class QuestionViewModel
    {
        [Required]
        public string CorrectAnswer { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        public bool AnsweredBeforeSugestion { get; set; }

        [Required]
        public double CalculatedPoints { get; set; }

        [Required]
        public double TimeForAnswerInSeconds { get; set; }
    }
}
