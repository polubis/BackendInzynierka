using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Services.Interfaces
{
    public interface IConfigurationManager
    {
        string GetValue(string key);
    }
}
