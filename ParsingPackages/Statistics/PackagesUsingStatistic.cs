using ParsingPackages.Application.PackagesFiles;
using ParsingPackages.Persistence.AzureDevOps;
using File = System.IO.File;

namespace ParsingPackages.Statistics
{
    public class PackagesUsingStatistic
    {
        private List<string[]> extensions { get; set; } = new List<string[]>() 
        {
            new string[]{ "pacakge.json" },
            new string[]{ "csproj", "packages.config" },
            new string[]{ "Dockerfile"}
        };
        private string[] groupNames { get; set; } = new string[] { "npm-packages", "NuGet-packages", "Dockerfiles" };

        public PackagesUsingStatistic()
        {
        }
        public PackagesUsingStatistic(List<string[]> ext, string[] groups) 
        {
            extensions = ext;
            groupNames = groups;
        }

        /// <summary>
        /// Получение статистики использования сторонних решений (пакетов) по всем проектам в указанной папке
        /// </summary>
        /// <param name="projectsDir">папка с проектами</param>
        /// <returns>статистика по группам пакетов</returns>
        public List<GroupStatisticData> getGlobalStatistic(string projectsDir)
        {
            List<GroupStatisticData> pakagesStatistic = new List<GroupStatisticData>();
            string[] projectDirectories= Directory.GetDirectories(projectsDir);
            foreach (string projectDirectory in projectDirectories)
            {
                List<string> packagesFiles = new PackagesFiles().getPackagesFiles(projectDirectory);
                Console.WriteLine($"Total files = {packagesFiles.Count}");

                List<GroupStatisticData> projectStatistics = getProjectPackagesStatistic(packagesFiles);
                pakagesStatistic = unionStatistics(pakagesStatistic, projectStatistics, projectDirectory);
            }
            return pakagesStatistic;
        }

        async public Task<List<GroupStatisticData>> getGlobalStatistic(AccessConfig accessConfig, bool isSaveFiles) 
        {
            List<GroupStatisticData> pakagesStatistic = new List<GroupStatisticData>();
            List<AzureDevOpsProject> projects = await new AzureDevOps(accessConfig).getProjects();
            foreach (AzureDevOpsProject project in projects)
            {
                List<AzureDevOpsRepository> repositories = await new AzureDevOps(accessConfig).getRepositories(project.projectId);

                foreach (AzureDevOpsRepository repository in repositories)
                {
                    List<AzureDevOpsItem> items = await new AzureDevOps(accessConfig).getRepositoryItems(project.projectId, repository.repositoryId);
                    repository.items = items;
                }
                project.repositories = repositories;

                List<GroupStatisticData> projectStatistics = await getProjectPackagesStatistic(accessConfig, project);
                pakagesStatistic = unionStatistics(pakagesStatistic, projectStatistics, project.projectId);

                //int waitBestTime = 3000;
                //Console.WriteLine($" --- Отдыхаем {waitBestTime} сек. ---");
                //Thread.Sleep(waitBestTime);
            }

            return pakagesStatistic;
        }

        private List<GroupStatisticData> unionStatistics(List<GroupStatisticData> pakagesStatistic, 
            List<GroupStatisticData> projectStatistics, string projectName)
        {
            if (pakagesStatistic == null || pakagesStatistic.Count == 0)
            {
                foreach (GroupStatisticData projectStat in projectStatistics)
                {
                    foreach (ItemStatisticData projectItem in projectStat.items)
                    {
                        projectItem.data.projects.Add(projectName);
                    }
                }
                return projectStatistics;
            }

            foreach (GroupStatisticData projectStat in projectStatistics)
            {
                foreach (GroupStatisticData packagesStat in pakagesStatistic)
                {
                    if (projectStat.groupName == packagesStat.groupName)
                    {
                        foreach (ItemStatisticData projectItem in projectStat.items)
                        {
                            ItemStatisticData? item = null;
                            item = packagesStat.items.Find(item => {
                                return item.data.Equals(projectItem.data);
                            });

                            if (item == null)
                            {
                                projectItem.data.projects.Add(projectName);
                                packagesStat.items.Add(new ItemStatisticData(projectItem.data, projectItem.total));
                            }
                            else
                            {
                                item.total += projectItem.total;
                                item.data.projects.Add(projectName);
                            }
                        }
                    }
                }
            }
            return pakagesStatistic;
        }

        public List<GroupStatisticData> getProjectPackagesStatistic(List<string> packagesFiles) 
        {
            List<GroupStatisticData> pakagesStatistic = new List<GroupStatisticData>();
            for (int i = 0; i < extensions.Count; i++)
            {
                GroupStatisticData group = new GroupStatisticData();   
                group.groupName = groupNames[i];
                group.groupExtensions = extensions[i];
                foreach (string pkgFile in packagesFiles) 
                {
                    group.updateProjectFileStatistic(pkgFile);                     
                }
                pakagesStatistic.Add(group);
            }
            return pakagesStatistic;
        }

        /// <summary>
        /// Получение использования сторонних решений (пакетов) в конкретном проекте
        /// </summary>
        /// <param name="accessConfig">конфигурация доступа в Azure DevOps</param>
        /// <param name="project">параметры проекта в Azure DevOps</param>
        /// <returns>Список с наименованиями пакетов и частотой их использования</returns>
        async public Task<List<GroupStatisticData>> getProjectPackagesStatistic(AccessConfig accessConfig, AzureDevOpsProject project)
        {
            List<GroupStatisticData> pakagesStatistic = new List<GroupStatisticData>();
            for (int i = 0; i < extensions.Count; i++)
            {
                GroupStatisticData group = new GroupStatisticData();
                group.groupName = groupNames[i];
                group.groupExtensions = extensions[i];
                foreach (AzureDevOpsRepository repository in project.repositories)
                {
                    foreach (AzureDevOpsItem item in repository.items)
                    {
                        await group.updateProjectFileStatistic(item.path, accessConfig, project.projectId, repository.repositoryId, item.objectId);
                    }
                }
                pakagesStatistic.Add(group);
            }
            return pakagesStatistic;
        }
    }
}
