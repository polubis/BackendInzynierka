using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class GetSoundsByCategoryDto:BaseDto
    {
        public List<string> SoundNames { get; set; }
    }
}
