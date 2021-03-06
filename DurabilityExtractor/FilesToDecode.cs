﻿using System;
using static System.Console;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BotwExtractor
{
    public class FilesToDecode
    {
        internal const string DecodeFolderPath = @"_decoded\";

        public FilesToDecode(FileInfo directory, BaseConfiguration baseConfiguration)
        {
            FilesList = Directory
                .EnumerateFiles(directory.DirectoryName, "*" + ".sbactorpack", SearchOption.AllDirectories);
            Configuration = baseConfiguration;
        }
        
        public void Decode()
        {
            Directory.CreateDirectory(Configuration.DefaultFolder + DecodeFolderPath);
            
            WriteLine("Decoding files");
            Parallel.ForEach(FilesList, (file) =>
            {
                FileInfo fileInfo = new FileInfo(file);
                string decodedFilesPath = Configuration.DefaultFolder + DecodeFolderPath
                                                                          + fileInfo.Name.Replace(".sbactor", ".bactor");
                
                ProcessStartInfo decodeProcess = new ProcessStartInfo();
                decodeProcess.FileName = Configuration.UnpackerExePath;
                decodeProcess.Arguments = $"/d {file} {decodedFilesPath}";
                decodeProcess.RedirectStandardOutput = true;
                decodeProcess.UseShellExecute = false;

                Process tp = new Process();
                tp.StartInfo = decodeProcess;
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
            WriteLine("Decoding done. Starting next step");
        }

        #region ForEach implementation (add 100seconds to execution)
        public void SingleThreadDecode()
        {
            foreach (var file in FilesList)
            {
                FileInfo fileInfo = new FileInfo(file);
                string decodedFilesPath = Configuration.DefaultFolder + DecodeFolderPath
                                                                          + fileInfo.Name.Replace(".sbactor", ".bactor");
                       
                ProcessStartInfo decodeProcess = new ProcessStartInfo();
                decodeProcess.FileName = Configuration.UnpackerExePath;
                decodeProcess.Arguments = $"/d {file} {decodedFilesPath}";
                decodeProcess.RedirectStandardOutput = true;
                decodeProcess.UseShellExecute = false;
       
                Process tp = new Process();
                tp.StartInfo = decodeProcess;
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
            } 
        }
        #endregion
        public IEnumerable<string> FilesList { get; set; }
        public BaseConfiguration Configuration { get; set; }
    }
}