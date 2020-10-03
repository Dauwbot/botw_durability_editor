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
        private const string ConvertedFolderPath = @"_converted\";
        private FileInfo _unpackedFolder;
        private IEnumerable<string> _foldersList;
        private IEnumerable<string> _filesList;
        private BaseConfiguration _configuration;

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
        
        public BaseConfiguration Configuration
        {
            get => _configuration;
            set => _configuration = value;
        }

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
                FileInfo dir = new FileInfo(folder);
                var bgparamslistFile =
                    Directory.EnumerateFiles(dir.FullName, "*" + ".bgparamlist", SearchOption.AllDirectories).FirstOrDefault();

                if (bgparamslistFile != null)
                {
                    FileInfo file = new FileInfo(bgparamslistFile);
                    string decodedFilePath = Configuration.DefaultFolder + ConvertedFolderPath
                                                                             + file.Name.Replace(".bgparamlist",
                                                                                 ".yml");

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
                        tp.WaitForExit();
                    }
                    catch (Exception e)
                    {
                        WriteLine(e);
                        throw;
                    }
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
    }
}