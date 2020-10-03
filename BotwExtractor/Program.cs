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
            const string decodedFolderPath = @"_decoded\";
            const string unpackedFolderPath = @"_unpacked\";
            const string convertedFolderPath = @"_converted\";
            DateTime executionStartTime = DateTime.Now;
            
            if (args.Length == 0)
            {
                WriteLine("Please specify a default folder containing BotwUnpacker.exe and the files to extract");
                Array.Resize(ref args, args.Length + 1);
                args[args.GetUpperBound(0)] = ReadLine();
            }
            
            BaseConfiguration baseConfiguration = new BaseConfiguration(args);
            string unpackerExe = baseConfiguration.UnpackerExePath;
            string converterExe = baseConfiguration.ConverterExePath;

            if (!baseConfiguration.ExeExists)
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

            Directory.CreateDirectory(baseConfiguration.DefaultFolder + decodedFolderPath);
            BaseFiles baseFilesList = new BaseFiles(baseConfiguration.DefaultFolder);

            WriteLine("Decoding files");
            Parallel.ForEach(baseFilesList.FilesList, (file) =>
            {
                FileInfo fileInfo = new FileInfo(file);
                string decodedFilesPath = baseConfiguration.DefaultFolder + decodedFolderPath
                                                                          + fileInfo.Name.Replace(".sbactor", ".bactor");
                
                ProcessStartInfo decodeProcess = new ProcessStartInfo();
                decodeProcess.FileName = unpackerExe;
                decodeProcess.Arguments = $"/d {file} {decodedFilesPath}";
                decodeProcess.RedirectStandardOutput = true;
                decodeProcess.UseShellExecute = false;

                Process tp = new Process();
                tp.StartInfo = decodeProcess;
                tp.EnableRaisingEvents = false;
                try
                {
                    tp.Start();
                }
                catch (Exception e)
                {
                    WriteLine(e);
                    throw;
                }
            });
            WriteLine("Decoding done. Starting next step");
            
            /*foreach (var file in baseFilesList.FilesList)
            {
                FileInfo fileInfo = new FileInfo(file);
                string decodedFilesPath = baseConfiguration.DefaultFolder + decodedFolderPath
                                                                          + fileInfo.Name.Replace(".sbactor", ".bactor");
                
                ProcessStartInfo decodeProcess = new ProcessStartInfo();
                decodeProcess.FileName = unpackerExe;
                decodeProcess.Arguments = $"/d {file} {decodedFilesPath}";
                decodeProcess.RedirectStandardOutput = true;
                decodeProcess.UseShellExecute = false;

                Process tp = new Process();
                tp.StartInfo = decodeProcess;
                tp.EnableRaisingEvents = false;
                try
                {
                    tp.Start();
                    currentFile++;
                    WriteLine($"Decoding file {currentFile}/{numberOfFiles}");
                    tp.WaitForExit();
                }
                catch (Exception e)
                {
                    WriteLine(e);
                    throw;
                }
            }*/

            Directory.CreateDirectory(baseConfiguration.DefaultFolder + unpackedFolderPath);
            DecodedFilesToUnpack decodedFiles = new DecodedFilesToUnpack(baseConfiguration.DefaultFolder + decodedFolderPath);

            WriteLine("Unpacking decoded files");
            Parallel.ForEach(decodedFiles.FilesList, (file) =>
            {
                FileInfo fileInfo = new FileInfo(file);
                string unpackedFilesPath = baseConfiguration.DefaultFolder + unpackedFolderPath;

                ProcessStartInfo unpackProcess = new ProcessStartInfo();
                unpackProcess.FileName = unpackerExe;
                unpackProcess.Arguments = $"/u {file} {unpackedFilesPath + fileInfo.Name.Replace(".bactorpack", "")}";
                unpackProcess.RedirectStandardOutput = true;
                unpackProcess.UseShellExecute = false;

                Process tp = new Process();
                tp.StartInfo = unpackProcess;
                tp.EnableRaisingEvents = false;
                try
                {
                    tp.Start();
                }
                catch (Exception e)
                {
                    WriteLine(e);
                    throw;
                }
            });
            WriteLine("Unpacking done. Starting next step");
            
            /*foreach (var file in decodedFiles.FilesList)
            {
                FileInfo fileInfo = new FileInfo(file);
                string unpackedFilesPath = baseConfiguration.DefaultFolder + unpackedFolderPath;
                
                ProcessStartInfo unpackProcess = new ProcessStartInfo();
                unpackProcess.FileName = unpackerExe;
                unpackProcess.Arguments = $"/u {file} {unpackedFilesPath + fileInfo.Name.Replace(".bactorpack", "")}";
                unpackProcess.RedirectStandardOutput = true;
                unpackProcess.UseShellExecute = false;

                Process tp = new Process();
                tp.StartInfo = unpackProcess;
                tp.EnableRaisingEvents = false;
                try
                {
                    tp.Start();
                    currentFile++;
                    WriteLine($"Unpacking file {currentFile}/{numberOfFiles}");
                    tp.WaitForExit();
                }
                catch (Exception e)
                {
                    WriteLine(e);
                    throw;
                }
            }*/
            
            Directory.CreateDirectory(baseConfiguration.DefaultFolder + convertedFolderPath);
            UnpackedFilesToConvert unpackedFiles = new UnpackedFilesToConvert(baseConfiguration.DefaultFolder + unpackedFolderPath);

            WriteLine("Converting from .bgparamlist to .yml");
            Parallel.ForEach(unpackedFiles.FoldersList, (folder) =>
            {
                FileInfo dir = new FileInfo(folder);
                var bgparamslistFile =
                    Directory.EnumerateFiles(dir.FullName, "*" + ".bgparamlist", SearchOption.AllDirectories).FirstOrDefault();

                if (bgparamslistFile != null)
                {
                    FileInfo file = new FileInfo(bgparamslistFile);
                    string decodedFilePath = baseConfiguration.DefaultFolder + convertedFolderPath
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
                    }
                    catch (Exception e)
                    {
                        WriteLine(e);
                        throw;
                    }
                }
            });
            WriteLine("Conversion done.");
            
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

            DateTime executionEndTime = DateTime.Now;
            TimeSpan executionTime = executionEndTime.Subtract(executionStartTime);
            WriteLine($"Execution time: {(int)executionTime.TotalSeconds} seconds");
        }
    }
}