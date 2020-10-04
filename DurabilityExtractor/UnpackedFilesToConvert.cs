using System;
using static System.Console;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BotwExtractor
{
    public class UnpackedFilesToConvert
    {
        internal const string ConvertedFolderPath = @"_converted\";

        public UnpackedFilesToConvert(string unpackedFolder, BaseConfiguration baseConfiguration)
        {
            UnpackedFolder = new FileInfo(unpackedFolder);
            FoldersList = Directory.GetDirectories(UnpackedFolder.DirectoryName);
            Configuration = baseConfiguration;
        }

        public void Convert()
        {
            Directory.CreateDirectory(Configuration.DefaultFolder + ConvertedFolderPath);
            WriteLine("Converting from .bgparamlist to .yml");
            
            Parallel.ForEach(FoldersList, (folder) =>
            {
                FileInfo folderFileInfo = new FileInfo(folder);
                var bgparamslistFile =
                    Directory.EnumerateFiles(folderFileInfo.FullName, "*" + ".bgparamlist", SearchOption.AllDirectories).FirstOrDefault();

                if (bgparamslistFile != null)
                {
                    FileInfo file = new FileInfo(bgparamslistFile);
                    Directory.CreateDirectory(Configuration.DefaultFolder + ConvertedFolderPath + folderFileInfo.Name);

                    ProcessStartInfo convertProcess = new ProcessStartInfo();
                    convertProcess.FileName = Configuration.ConverterExePath;
                    convertProcess.Arguments = $"{bgparamslistFile}";
                    convertProcess.WindowStyle = ProcessWindowStyle.Hidden;

                    Process tp = new Process();
                    tp.StartInfo = convertProcess;
                    tp.EnableRaisingEvents = true;
                    try
                    {
                        tp.Start();
                        tp.WaitForExit();
                    }
                    catch (Exception e)
                    {
                        WriteLine(e);
                        throw;
                    }

                    bgparamslistFile = bgparamslistFile + ".xml";
                    string decodedFilePath =
                        Configuration.DefaultFolder + ConvertedFolderPath + folderFileInfo.Name + @"\" +
                        file.Name + ".xml";
                    File.Move(bgparamslistFile, decodedFilePath);
                }
            });
            WriteLine("Conversion done.");
            
            #region ForEach implementation (add 100seconds to execution)

            /*foreach (var folder in unpackedFiles.FoldersList)
           {
               FileInfo dir = new FileInfo(folder);
               var bgparamslistFile =
                   Directory.EnumerateFiles(dir.FullName, "*" + ".bgparamlist", SearchOption.AllDirectories).FirstOrDefault();

               if (bgparamslistFile != null)
               {
                   FileInfo file = new FileInfo(bgparamslistFile);
                   string decodedFilePath = baseConfiguration.DefaultFolder + convertedFolderPath
                                                                             + file.Name.Replace(".bgparamlist", ".yml");

                   ProcessStartInfo convertProcess = new ProcessStartInfo();
                   convertProcess.FileName = "powershell.exe";
                   convertProcess.Arguments = $"py -m aamp {bgparamslistFile} {decodedFilePath}";
                   convertProcess.WindowStyle = ProcessWindowStyle.Hidden;

                   Process tp = new Process();
                   tp.StartInfo = convertProcess;
                   tp.EnableRaisingEvents = true;
                   try
                   {
                       tp.Start();
                       currentFile++;
                       WriteLine($"Converting file {currentFile}/{numberOfFiles}");
                       tp.WaitForExit();
                   }
                   catch (Exception e)
                   {
                       WriteLine(e);
                       throw;
                   }
               }
           }*/

            #endregion
        }
        public FileInfo UnpackedFolder { get; set; }
        public IEnumerable<string> FoldersList { get; set; }
        public BaseConfiguration Configuration { get; set; }
    }
}