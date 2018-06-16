using Inzynierka.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Services.Services
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfiguration _configuration;
        public ConfigurationManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetValue(string key)
        {
            return _configuration[key];
        }
    }
}
