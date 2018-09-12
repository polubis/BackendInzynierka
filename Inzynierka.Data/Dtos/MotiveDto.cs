using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class MotiveDto:BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string MainColor { get; set; }

        public string FontColor { get; set; }
    }
}
