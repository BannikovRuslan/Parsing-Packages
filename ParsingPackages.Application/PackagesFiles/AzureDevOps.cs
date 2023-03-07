using ParsingPackages.Logging;
using ParsingPackages.Persistence.AzureDevOps;
using ParsingPackages.Statistics;
using ParsingPackages.Utils;
using System.Text.Json;

namespace ParsingPackages.Application.PackagesFiles
{
    public class AzureDevOps
    {
        private AccessConfig accessConfig;

        public AzureDevOps(AccessConfig accessConfig) 
        { 
            this.accessConfig = accessConfig;
        }

        async public Task<List<AzureDevOpsProject>> getProjects()
        {
            List<JsonParseData> jsonParseData = new List<JsonParseData>() {
                new JsonParseData("name"),
            };

            List<AzureDevOpsProject> projects = new List<AzureDevOpsProject>();
            string json = await new Requests(accessConfig).GetProjects();
            if (json.Length == 0)
            {
                string message = "<getProjects> -> Данные не были получены!\n" +
                                 "Информация по запросу:\n" + 
                                 $"\tКоллекция: {accessConfig.collection}";
                new Logger().warningLogger(message, true, true);
                return projects;
            }

            JsonDocument doc = JsonDocument.Parse(json);
            JsonElement value = doc.RootElement.GetProperty("value");
            
            foreach (var item in value.EnumerateArray()) 
            {
                List<ItemData> items = ParsersHelpers.jsonParser(item.ToString(), jsonParseData);
                foreach (ItemData p in items)
                {
                    AzureDevOpsProject project = new AzureDevOpsProject(p.values[1]);
                    projects.Add(project);
                }
            }
            return projects;
        }

        async public Task<List<AzureDevOpsRepository>> getRepositories(string projectName)
        {
            List<JsonParseData> jsonParseData = new List<JsonParseData>() {
                new JsonParseData("name"),
            };

            List<AzureDevOpsRepository> repositories = new List<AzureDevOpsRepository>();
            string json = await new Requests(accessConfig).GetRepositories(projectName);
            if (json.Length == 0)
            {
                string message = "<getRepositories> -> Данные не были получены!\n"+
                                 "Информация по запросу:\n"+
                                 $"\tКоллекция: {accessConfig.collection}\n"+
                                 $"\tПроект: {projectName}";
                new Logger().warningLogger(message, true, true);
                return repositories;
            }

            JsonDocument doc = JsonDocument.Parse(json);
            JsonElement value = doc.RootElement.GetProperty("value");
 
            foreach (var item in value.EnumerateArray())
            {
                List<ItemData> items = ParsersHelpers.jsonParser(item.ToString(), jsonParseData);
                foreach (ItemData r in items)
                {
                    AzureDevOpsRepository repository = new AzureDevOpsRepository(r.values[1]);
                    repositories.Add(repository);
                }
            }
            return repositories;
        }

        async public Task<List<AzureDevOpsItem>> getRepositoryItems(string projectName, string repositoryName)
        {
            List<JsonParseData> jsonParseData = new List<JsonParseData>() {
                new JsonParseData("objectId"),
                new JsonParseData("path"),
            };

            List<AzureDevOpsItem> repositoryItems = new List<AzureDevOpsItem>();
            string json = await new Requests(accessConfig).GetItems(projectName, repositoryName);
            if (json.Length == 0)
            {
                string message = "<getRepositoryItems> -> Данные не были получены!\n"+
                                 "Информация по запросу:\n"+
                                 $"\tКоллекция: {accessConfig.collection}\n"+
                                 $"\tПроект: {projectName}\n"+
                                 $"\tРепозиторий: {repositoryName}";
                new Logger().warningLogger(message, true, true);
                return repositoryItems;
            }

            JsonDocument doc = JsonDocument.Parse(json);
            JsonElement value = doc.RootElement.GetProperty("value");
            foreach (var item in value.EnumerateArray())
            {
                string file = item.GetProperty("path").ToString();
                if (PackagesFiles.isPackageJsonFile(file) 
                    || PackagesFiles.isCprojFile(file) || PackagesFiles.isPackagesConfigFile(file)
                    || PackagesFiles.isDockerFile(file))
                {
                    List<ItemData> items = ParsersHelpers.jsonParser(item.ToString(), jsonParseData);
                    for (int i = 0; i < items.Count(); i += jsonParseData.Count()) 
                    {
                        AzureDevOpsItem itemR = new AzureDevOpsItem(items[i].values[1], items[i + 1].values[1]);
                        repositoryItems.Add(itemR);
                    }
                }
                
            }
            return repositoryItems;
        }
    }
}
