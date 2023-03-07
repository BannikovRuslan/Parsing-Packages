using ParsingPackages.Application.PackagesFiles;
using ParsingPackages.Logging;
using ParsingPackages.Persistence.AzureDevOps;
using ParsingPackages.Utils;


namespace ParsingPackages.Statistics
{
    public class GroupStatisticData
    {
        private List<XmlParseData> CSPROJ_PARSE_DATA { get; set; } = new List<XmlParseData>() 
        { 
            new XmlParseData("Reference", new string[] { "Include" }),
            new XmlParseData("PackageReference", new string[] { "Include" }),
            new XmlParseData("Reference", new string[] { "Update" }),
            new XmlParseData("PackageReference", new string[] { "Update" }),
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

        public string? groupName { get; set; }
        public string[] groupExtensions { get; set; } = { };
        public List<ItemStatisticData> items { get; set; } = new List<ItemStatisticData>();

        public void updateProjectFileStatistic(string filePath) 
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            
            string fileData = File.ReadAllText(filePath);
            updateGroupsStatistic(filePath, fileData);
        }

        /// <summary>
        /// Обновление статистики по конкретному проекту по данным из соответствующего файла
        /// </summary>
        /// <param name="filePath">путь к файлу в репозитории</param>
        /// <param name="accessConfig">параметры доступа в Azure DevOps</param>
        /// <param name="projectId">идентификатор / имя проекта</param>
        /// <param name="repositoryId">идентификатор / имя репозитория</param>
        /// <param name="objectId">идентификатор файла</param>
        /// <returns></returns>
        async public Task updateProjectFileStatistic(string filePath,
            AccessConfig accessConfig, string projectId, string repositoryId, string objectId) 
        {
            string fileData = await new Requests(accessConfig).GetFile(projectId, repositoryId, objectId);
            if (fileData.Length == 0) {
                string message = "<updateProjectFileStatistic> -> Данные не были получены!\n"+
                                 "Информация по запросу:\n"+
                                 $"\tФайл: {filePath}\n"+
                                 $"\tПроект: {projectId}\n"+
                                 $"\tРепозиторий: {repositoryId}\n"+
                                 $"\tИдентификатор файла: {objectId}\n";
                new Logger().warningLogger(message, true, true);
                return;
            }
            updateGroupsStatistic(filePath, fileData);
        }

        /// <summary>
        /// Обновление статистики по группе в зависимости от файла пакета
        /// </summary>
        /// <param name="filePath">имя файла</param>
        /// <param name="fileData">содержимое файла</param>
        private void updateGroupsStatistic(string filePath, string fileData) 
        {
            foreach (string groupExtension in groupExtensions)
            {
                switch (groupExtension)
                {
                    case "csproj":
                        if (PackagesFiles.isCprojFile(filePath))
                        {
                            initProjectGroupStatistic(getProjectNuGetPackages(fileData, CSPROJ_PARSE_DATA));
                        }
                        break;
                    case "packages.config":
                        if (PackagesFiles.isPackagesConfigFile(filePath))
                        {
                            initProjectGroupStatistic(getProjectNuGetPackages(fileData, PACKAGES_CONFIG_PARSE_DATA));
                        }
                        break;
                    case "pacakge.json":
                        if (PackagesFiles.isPackageJsonFile(filePath))
                        {
                            initProjectGroupStatistic(getProjectNpmPackagesStatistic(fileData, PACKAGES_JSON_PARSE_DATA));
                        }
                        break;
                    case "Dockerfile":
                        if (PackagesFiles.isDockerFile(filePath))
                        {
                            initProjectGroupStatistic(getProjectDockerPackagesStatistic(fileData, DOCKERFILE_PARSE_DATA));
                        }
                        break;
                    default:
                        break;
                }
            }
        }


        private List<ItemData> getProjectNuGetPackages(string fileData, List<XmlParseData> parseData)
        {
            return ParsersHelpers.xmlParser(fileData, parseData);
        }

        private List<ItemData> getProjectNpmPackagesStatistic(string fileData, List<JsonParseData> parseData) 
        {
            return ParsersHelpers.jsonParser(fileData, parseData);
        }

        private List<ItemData> getProjectDockerPackagesStatistic(string fileData, List<DockerfileParseData> parseData)
        {
            return ParsersHelpers.dockerfileParser(fileData, parseData);
        }

        private void initProjectGroupStatistic(List<ItemData> packagesData)
        {
            foreach (ItemData packageData in packagesData)
            {
                ItemStatisticData? item = null;
                if (items.Count > 0)
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
