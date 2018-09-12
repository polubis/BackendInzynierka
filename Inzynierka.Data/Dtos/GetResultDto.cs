using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inzynierka.Data.DbModels;

namespace Inzynierka.Data.Dtos
{
    public class GetResultDto:BaseDto
    {
        public List<QuizDto> Quizes { get; set; }
        public RateDto Rate { get; set; }

        public GetResultDto(List<QuizDto> Quizes, RateDto Rate)
        {
            this.Quizes = Quizes;
            this.Rate = Rate;
        }
    }
}
