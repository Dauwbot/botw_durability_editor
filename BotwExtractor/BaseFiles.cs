using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BotwExtractor
{
    public class BaseFiles
    {
        public IEnumerable<string> FilesList
        {
            get => _filesList;
            set => _filesList = value;
        }
        
        private IEnumerable<string> _filesList;
        public BaseFiles(FileInfo directory)
        {
            FilesList = Directory
                .EnumerateFiles(directory.DirectoryName, "*" + ".sbactorpack", SearchOption.AllDirectories);
        }

        
    }
}