﻿using System;
using static System.Console;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BotwExtractor
{
    public class DecodedFilesToUnpack
    {
        internal const string UnpackFolderPath = @"_unpacked\";

        public DecodedFilesToUnpack(string decodedFolder, BaseConfiguration baseConfiguration)
        {
            DecodedFolder = new FileInfo(decodedFolder);
            FilesList = Directory
                .EnumerateFiles(DecodedFolder.DirectoryName, "*" + ".bactorpack", SearchOption.AllDirectories);
            Configuration = baseConfiguration;
        }

        public void Unpack()
        {
            Directory.CreateDirectory(Configuration.DefaultFolder + UnpackFolderPath);
            
            WriteLine("Unpacking decoded files");
            Parallel.ForEach(FilesList, (file) =>
            {
                FileInfo fileInfo = new FileInfo(file);
                string unpackedFilesPath = Configuration.DefaultFolder + UnpackFolderPath;

                ProcessStartInfo unpackProcess = new ProcessStartInfo();
                unpackProcess.FileName = Configuration.UnpackerExePath;
                unpackProcess.Arguments = $"/u {file} {unpackedFilesPath + fileInfo.Name.Replace(".bactorpack", "")}";
                unpackProcess.RedirectStandardOutput = true;
                unpackProcess.UseShellExecute = false;

                Process tp = new Process();
                tp.StartInfo = unpackProcess;
                tp.EnableRaisingEvents = false;
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
            });
            WriteLine("Unpacking done. Starting next step");

            #region ForEach implementation (add 100seconds to execution)
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
            #endregion
        }
        public FileInfo DecodedFolder { get; set; }
        public IEnumerable<string> FilesList { get; set; }
        public BaseConfiguration Configuration { get; set; }
    }
}