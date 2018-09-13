using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class RatesDto:BaseDto
    {
        public List<RateDto> Rates { get; set; }
    }
}
