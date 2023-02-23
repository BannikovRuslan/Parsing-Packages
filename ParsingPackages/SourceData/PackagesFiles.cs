using System;
using System.Collections.Generic;
using System.IO;

namespace ParsingPackages.SourceData
{
    public class PackagesFiles
    {
        const string PACKAGE_JSON_ID = "package.json";
        const string CSPROJ_ID = "csproj";
        const string PACKAGES_CONFIG_ID = "packages.config";
        const string DOCKERFILE_ID = "Dockerfile";

        private string[] extensions { get; set; } = new string[] { 
            PACKAGE_JSON_ID, CSPROJ_ID, PACKAGES_CONFIG_ID, DOCKERFILE_ID
        };
        private string[] masks { get; set; } = new string[] { 
            PACKAGE_JSON_ID, "*." + CSPROJ_ID, PACKAGES_CONFIG_ID, DOCKERFILE_ID + ".*" 
        };
        public PackagesFiles()
        {
        }

        public PackagesFiles(string[] extensions)
        {
            this.extensions = extensions;
        }

        public List<string> getPackagesFiles(string searchPath)
        {
            if (searchPath.Length == 0)
            {
                searchPath = Directory.GetCurrentDirectory();
            }
            Console.WriteLine("Search files in '" + searchPath + "' ...");

            List<string> packagesFiles = new List<string>();
            if (Directory.Exists(searchPath))
            {
                foreach (string mask in masks)
                {
                    packagesFiles.AddRange(Directory.GetFiles(searchPath, mask));
                }
                
            }

            return packagesFiles;
        }

        public static bool isPackageJsonFile(string pathFile) 
        {
            return Path.GetFileName(pathFile) == PACKAGE_JSON_ID;
        }

        public static bool isCprojFile(string pathFile)
        {
            return Path.GetExtension(pathFile) == "." + CSPROJ_ID;
        }

        public static bool isPackagesConfigFile(string pathFile)
        {
            return Path.GetFileName(pathFile) == PACKAGES_CONFIG_ID;
        }

        public static bool isDockerFile(string pathFile)
        {
            return Path.GetFileNameWithoutExtension(pathFile) == DOCKERFILE_ID;
        }
    }
}
