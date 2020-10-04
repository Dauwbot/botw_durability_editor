using System;
using System.Threading;
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

            // FilesToDecode filesToDecode = new FilesToDecode(baseConfiguration.DefaultFolder, baseConfiguration);
            // filesToDecode.Decode();
            //
            // DecodedFilesToUnpack decodedFiles =
            //     new DecodedFilesToUnpack(baseConfiguration.DefaultFolder + FilesToDecode.DecodeFolderPath,
            //         baseConfiguration);
            // decodedFiles.Unpack();
            
            // UnpackedFilesToConvert unpackedFiles =
            //     new UnpackedFilesToConvert(baseConfiguration.DefaultFolder + DecodedFilesToUnpack.UnpackFolderPath,
            //         baseConfiguration);
            // unpackedFiles.Convert();
            
            WriteLine("By how much do you want to increase the durability? -1 for Infinite");
            double newDurability;
            double.TryParse(ReadLine(), out newDurability);
            
            YamlFilesEditor yamlEditor =
                new YamlFilesEditor(baseConfiguration.DefaultFolder + UnpackedFilesToConvert.ConvertedFolderPath , baseConfiguration, newDurability);
            yamlEditor.UpdateDurability();
            

            DateTime executionEndTime = DateTime.Now;
            TimeSpan executionTime = executionEndTime.Subtract(executionStartTime);
            WriteLine($"Execution time: {(int)executionTime.TotalSeconds} seconds.");
            WriteLine("\r\nProgram will now exit.");
            Thread.Sleep(5000);
        }
    }
}