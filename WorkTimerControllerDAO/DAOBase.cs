using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WorkTimeControllerDAO
{
    public class DAOBase
    {
        #region Constants
        private string folderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WorkTimeController");
        public const string fileName = "WorkTimeControllerSummary.txt"; 
	    #endregion

        #region Properties
		public string FullPath 
        { 
            get 
            {
                return Path.Combine(folderPath, fileName);
            }
        } 
	    #endregion

        #region Methods
        private bool CheckFolderExistence()
        {
            return Directory.Exists(folderPath);
        }

        private bool CheckFileExistence()
        {
            return File.Exists(FullPath);
        }

        private void CreateFile()
        {
            if (!CheckFileExistence())
            {
                StreamWriter sWriter = File.CreateText(FullPath);
                sWriter.Close();
            }
        }

        private void CreateFolder()
        {
            if (!CheckFolderExistence())
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public string ReadFile()
        {
            string result = string.Empty;
            
            if (CheckFileExistence())
            {
                FileInfo file = new FileInfo(FullPath);
                using (StreamReader reader = file.OpenText())
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }

        public void WriteFile(string textToWrite)
        {
            CreateFolder();
            CreateFile();

            FileInfo file = new FileInfo(FullPath);
            using (StreamWriter writer = file.CreateText())
            {
                writer.Write(textToWrite);
            }
        }
        #endregion
    }
}
