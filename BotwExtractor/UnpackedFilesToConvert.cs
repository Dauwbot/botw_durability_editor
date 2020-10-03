using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BotwExtractor
{
    public class UnpackedFilesToConvert
    {
        private FileInfo _unpackedFolder;
        private IEnumerable<string> _foldersList;
        private IEnumerable<string> _filesList;

        public FileInfo UnpackedFolder
        {
            get => _unpackedFolder;
            set => _unpackedFolder = value;
        }

        public IEnumerable<string> FoldersList
        {
            get => _foldersList;
            set => _foldersList = value;
        }

        public IEnumerable<string> FilesList
        {
            get => _filesList;
            set => _filesList = value;
        }

        public UnpackedFilesToConvert(string unpackedFolder)
        {
            UnpackedFolder = new FileInfo(unpackedFolder);
            FoldersList = Directory.GetDirectories(UnpackedFolder.DirectoryName);
        }
    }
}