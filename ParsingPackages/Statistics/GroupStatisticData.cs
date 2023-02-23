using ParsingPackages.SourceData;
using ParsingPackages.Utils;
using System.Collections.Generic;

namespace ParsingPackages.Statistics
{
    public class GroupStatisticData
    {
        private List<XmlParseData> CSPROJ_PARSE_DATA { get; set; } = new List<XmlParseData>() 
        { 
            new XmlParseData("Reference", new string[] { "Include" }),
            new XmlParseData("PackageReference", new string[] { "Include" }),
        };

        private List<XmlParseData> PACKAGES_CONFIG_PARSE_DATA { get; set; } = new List<XmlParseData>()
        {
            new XmlParseData("package", new string[] { "id" }),
        };

        private List<JsonParseData> PACKAGES_JSON_PARSE_DATA { get; set; } = new List<JsonParseData>()
        {
            new JsonParseData("dependencies"), 
            new JsonParseData("devDependencies"), 
            new JsonParseData("peerDependencies"),
        };

        private List<DockerfileParseData> DOCKERFILE_PARSE_DATA { get; set; } = new List<DockerfileParseData>()
        {
            new DockerfileParseData("FROM"),
        };

        public string groupName { get; set; }
        public string[] groupExtensions { get; set; }
        public List<ItemStatisticData> items { get; set; } = new List<ItemStatisticData>();

        public void updateProjectStatistic(string filePath) 
        {
            foreach (string groupExtension in groupExtensions)
            {
                switch (groupExtension)
                {
                    case "csproj":
                        if (PackagesFiles.isCprojFile(filePath)) 
                        {
                            initProjectGroupStatistic(getProjectNuGetPackages(filePath, CSPROJ_PARSE_DATA));
                        }                   
                        break;
                    case "packages.config":
                        if (PackagesFiles.isPackagesConfigFile(filePath))
                        {
                            initProjectGroupStatistic(getProjectNuGetPackages(filePath, PACKAGES_CONFIG_PARSE_DATA));
                        }
                        break;
                    case "pacakge.json":
                        if (PackagesFiles.isPackageJsonFile(filePath))
                        {
                            initProjectGroupStatistic(getProjectNpmPackagesStatistic(filePath, PACKAGES_JSON_PARSE_DATA));
                        }
                        break;
                    case "Dockerfile":
                        if (PackagesFiles.isDockerFile(filePath))
                        {
                            initProjectGroupStatistic(getProjectDockerPackagesStatistic(filePath, DOCKERFILE_PARSE_DATA));
                        }
                        break;
                    default:
                        break;
                }
            } 

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        private List<ItemData> getProjectNuGetPackages(string filePath, List<XmlParseData> parseData)
        {
            return FileParsersHelpers.xmlParser(filePath, parseData);
        }

        private List<ItemData> getProjectNpmPackagesStatistic(string filePath, List<JsonParseData> parseData) 
        {
            return FileParsersHelpers.jsonParser(filePath, parseData);
        }

        private List<ItemData> getProjectDockerPackagesStatistic(string filePath, List<DockerfileParseData> parseData)
        {
            return FileParsersHelpers.dockerfileParser(filePath, parseData);
        }

        private void initProjectGroupStatistic(List<ItemData> packagesData)
        {
            foreach (ItemData packageData in packagesData)
            {
                ItemStatisticData item = null;
                if (items != null)
                {
                    item = items.Find(item => {
                        return item.data.Equals(packageData);
                    });
                }

                if (item == null)
                {
                    item = new ItemStatisticData(packageData, 1);
                    items.Add(item);
                }
            }
        }
    }
}
