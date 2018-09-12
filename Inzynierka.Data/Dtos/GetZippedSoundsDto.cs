using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Inzynierka.Data.Dtos
{
    public class GetZippedSoundsDto:BaseDto
    {
        public string Path { get; set; }
    }
}
