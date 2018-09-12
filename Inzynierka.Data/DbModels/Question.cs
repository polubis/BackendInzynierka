using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inzynierka.Data.DbModels
{
    public class Question:Entity
    {
        [Required]
        public int QuizId { get; set; }

        [Required]
        public string CorrectAnswer { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        public bool WasAnswerCorrect { get; set; }


        [Required]
        public int TimeForAnswerInSeconds { get; set; }

        [Required]
        public virtual Quiz Quiz { get; set; }

        // Dodac requesty od obslugi interwalow 

    }
}
