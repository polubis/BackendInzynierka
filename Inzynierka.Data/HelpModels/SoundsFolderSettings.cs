using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.HelpModels
{
    public class SoundsFolderSettings
    {
        public int NumberOfFiles { get; set; }
        public string SoundType { get; set; }

        public SoundsFolderSettings(int NumberOfFiles, string SoundType)
        {
            this.NumberOfFiles = NumberOfFiles;
            this.SoundType = SoundType;
        }
    }
}
