using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BotwExtractor
{
    public class DecodedFilesToUnpack
    {
        private FileInfo _decodedFolder;
        private IEnumerable<string> _filesList;

        public FileInfo DecodedFolder
        {
            get => _decodedFolder;
            set => _decodedFolder = value;
        }

        public IEnumerable<string> FilesList
        {
            get => _filesList;
            set => _filesList = value;
        }

        public DecodedFilesToUnpack(string decodedFolder)
        {
            DecodedFolder = new FileInfo(decodedFolder);
            FilesList = Directory
                .EnumerateFiles(DecodedFolder.DirectoryName, "*" + ".bactorpack", SearchOption.AllDirectories);
        }
    }
}