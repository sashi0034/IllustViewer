using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IllustViewer
{
    public class StragePath
    {
        private readonly string directoryPath;
        public string DirectoryPath => directoryPath;
        public string DirectoryFullPath => System.IO.Path.GetFullPath(directoryPath);

        public StragePath()
        {
            directoryPath = "./strage";
            CheckMakeNewDirectory();
        }

        public string MakeFilePath(string fileName)
        {
            return directoryPath + "/" + fileName;
        }

        public void CheckMakeNewDirectory()
        {
           if (System.IO.File.Exists(directoryPath)) return;

           System.IO.Directory.CreateDirectory(directoryPath);
        }
    }
}
