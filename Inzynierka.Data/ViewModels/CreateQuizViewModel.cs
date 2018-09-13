using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Inzynierka.Data.CustomValidators;
using Inzynierka.Data.DbModels;

namespace Inzynierka.Data.ViewModels
{
    public class CreateQuizViewModel
    {
        [MustBeCorrectCategoryName(ErrorMessage = "Nazwa kategori powinna zgadzać się ze zdefiniowanymi nazwami")]
        public string QuizType { get; set; }

        [Required]
        public int NumberOfPositiveRates { get; set; }

        [Required]
        public int NumberOfNegativeRates { get; set; }

        [MustHaveCorrectQuestionsNumber(ErrorMessage = "Liczba pytań nie zgadza się ze zdefiniowanymi liczbami pytań")]
        public List<QuestionViewModel> Questions { get; set; }

    }
}
