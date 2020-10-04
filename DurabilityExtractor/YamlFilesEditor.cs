using System;
using System.Collections;
using System.Collections.Generic;
using static System.Console;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using YamlDotNet.Serialization;

namespace BotwExtractor
{
    public class YamlFilesEditor
    {
        public FileInfo ConvertedFolder { get; set; }
        public string[] FoldersList { get; set; }
        public IEnumerable<string> FilesList { get; set; }
        public BaseConfiguration Configuration { get; set; }
        public double NewDurability { get; set; }

        public YamlFilesEditor(string convertedFolder, BaseConfiguration baseConfiguration, double newDurability)
        {
            ConvertedFolder = new FileInfo(convertedFolder);
            Configuration = baseConfiguration;
            FoldersList = Directory.GetDirectories(ConvertedFolder.DirectoryName);
            NewDurability = newDurability;

        }

        public void UpdateDurability()
        {
            var filesList = Directory
                .EnumerateFiles(ConvertedFolder.DirectoryName, "*" + ".xml", SearchOption.AllDirectories);
            Parallel.ForEach(filesList, (file) =>
            {
                FileInfo fileInfo = new FileInfo(file);
                XmlDocument fileXml = new XmlDocument();
                fileXml.Load(fileInfo.FullName);
                XmlElement root = fileXml.DocumentElement;
                
                if (root != null)
                {
                    XmlNode durabilityNode = root.SelectSingleNode("//General/Life");
                    if (durabilityNode != null)
                    {
                        string lifeValue = durabilityNode.InnerText;
                        if (double.TryParse(lifeValue, out var currentDurability))
                        {
                            if (NewDurability < 0)
                            {
                                lifeValue =  (currentDurability / Math.Abs(NewDurability)).ToString();
                            }
                            else
                            {
                                lifeValue =  (currentDurability * NewDurability).ToString();
                            }
                            
                            if (double.Parse(lifeValue) < 1)
                            {
                                lifeValue = "1";
                            }

                            durabilityNode.InnerText = lifeValue;
                            fileXml.Save(fileInfo.FullName);
                        }
                    }
                }
            });
        }
    }
}