using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace BotwExtractor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            DateTime executionStartTime = DateTime.Now;
            
            if (args.Length == 0)
            {
                WriteLine("Please specify a default folder containing BotwUnpacker.exe and the files to extract");
                Array.Resize(ref args, args.Length + 1);
                args[args.GetUpperBound(0)] = ReadLine();
            }
            
            BaseConfiguration baseConfiguration = new BaseConfiguration(args);
            baseConfiguration.CheckExe();

            BaseFiles baseFilesList = new BaseFiles(baseConfiguration.DefaultFolder, baseConfiguration);
            baseFilesList.Decode();

            DecodedFilesToUnpack decodedFiles = new DecodedFilesToUnpack(baseConfiguration.DefaultFolder + baseFilesList.decodedFolderPath, baseConfiguration);
            decodedFiles.Unpack();
            
            UnpackedFilesToConvert unpackedFiles = new UnpackedFilesToConvert(baseConfiguration.DefaultFolder + decodedFiles.unpackedFolderPath, baseConfiguration);
            unpackedFiles.Convert();

            DateTime executionEndTime = DateTime.Now;
            TimeSpan executionTime = executionEndTime.Subtract(executionStartTime);
            WriteLine($"Execution time: {(int)executionTime.TotalSeconds} seconds.");
            WriteLine("\r\nProgram will now exit.");
            Thread.Sleep(5000);
        }
    }
}