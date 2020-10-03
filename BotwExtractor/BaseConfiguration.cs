using static System.Console;
using System.IO;

namespace BotwExtractor
{
    public class BaseConfiguration
    {
        private const string extractorExe = "BotwUnpacker.exe";
        private const string converterExe = "aampTool.exe";
        private string[] _args;
        private FileInfo _defaultFolder;
        private bool _exeExists;
        private string _unpackerExePath;
        private string _converterExePath;
        
        public string[] Args
        {
            get => _args;
            set => _args = value;
        }

        public FileInfo DefaultFolder
        {
            get => _defaultFolder;
            set => _defaultFolder = value;
        }
        
        public bool ExeExists
        {
            get => _exeExists;
            set => _exeExists = value;
        }

        public string UnpackerExePath
        {
            get => _unpackerExePath;
            set => _unpackerExePath = value;
        }

        public string ConverterExePath
        {
            get => _converterExePath;
            set => _converterExePath = value;
        }

        public BaseConfiguration(string[] args)
        {
            Args = args;
            var folder = Args.GetValue(0).ToString() + @"\";
            UnpackerExePath = $@"{folder + extractorExe}";
            ConverterExePath = $@"{folder + converterExe}";
            ExeExists = File.Exists(UnpackerExePath);
            //TODO CREATE ARRAY TO CHECK THE EXISTENCE OF THE EXES
            DefaultFolder = new FileInfo(folder);
        }

        public void CheckExe()
        {
            if (!ExeExists)
            {
                WriteLine("The required extractor .exe does not exists. Do you want to download it? [Y/N]");
                char response = ReadKey().KeyChar;
                if (response == 'y' || response == 'Y')
                {
                    WriteLine("\r\nOK");
                }
                else
                {
                    WriteLine("\r\nAre you sure? If you don't download this program will exit");
                }
            }
        }
    }
}