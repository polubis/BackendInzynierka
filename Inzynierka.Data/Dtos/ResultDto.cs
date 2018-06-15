﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class ResultDto<T> where T : BaseDto
    {
        public T SuccessResult { get; set; }
        public string Error { get; set; }
    }
}
