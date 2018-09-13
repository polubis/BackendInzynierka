using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class GetMotivesDto:BaseDto
    {
        public List<GetMotiveDto> GetMotivesDtoList { get; set; }
    }
}
