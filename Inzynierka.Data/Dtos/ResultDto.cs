using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class ResultDto<T> where T : BaseDto
    {
        public T SuccessResult { get; set; }
        public List<string> Errors { get; set; }
        public bool IsError { get { return Errors.Count > 0; } }
        public ResultDto()
        {
            Errors = new List<string>();
        }

    }
}
