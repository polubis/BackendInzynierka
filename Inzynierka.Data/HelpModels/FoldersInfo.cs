using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.HelpModels
{
    public class FoldersInfo
    {
        public int NumberOfAllFiles { get; set; }
        public Dictionary<string, int> NumberOfFilesByCategory { get; set; }
        public bool isAllFoldersHaveMinimumNumberOfFiles { get; set; }
        public FoldersInfo(Dictionary<string,int> NumberOfFilesByCategory, int minimumLengthInCategory)
        {
            this.NumberOfFilesByCategory = NumberOfFilesByCategory;
            this.NumberOfAllFiles = countNumberOfAllFiles(NumberOfFilesByCategory);
            this.isAllFoldersHaveMinimumNumberOfFiles = checkIsAllFoldersHaveMinimumNumberOfFiles(NumberOfFilesByCategory, minimumLengthInCategory);
        }
        private int countNumberOfAllFiles(Dictionary<string, int> NumberOfFilesByCategory)
        {
            int counter = 0;
            foreach(var element in NumberOfFilesByCategory)
            {
                counter += element.Value;
            }

            return counter;
        }
        private bool checkIsAllFoldersHaveMinimumNumberOfFiles(Dictionary<string, int> NumberOfFilesByCategory, int minimumLength)
        {
            foreach (var element in NumberOfFilesByCategory)
            {
                if (element.Value < minimumLength)
                    return false;
            }

            return true;
        }
    }
}
