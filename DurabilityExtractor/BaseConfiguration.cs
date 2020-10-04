using static System.Console;
using System.IO;

namespace BotwExtractor
{
    public class BaseConfiguration
    {
        private const string ExtractorExe = "BotwUnpacker.exe";
        private const string ConverterExe = "aampTool.exe";

        public BaseConfiguration(string[] args)
        {
            Args = args;
            var folder = Args.GetValue(0).ToString() + @"\";
            UnpackerExePath = $@"{folder + ExtractorExe}";
            ConverterExePath = $@"{folder + ConverterExe}";
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
        
        public string[] Args { get; set; }
        public FileInfo DefaultFolder { get; set; }
        public bool ExeExists { get; set; }
        public string UnpackerExePath { get; set; }
        public string ConverterExePath { get; set; }
    }
}