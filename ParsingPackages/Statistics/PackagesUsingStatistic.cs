using ParsingPackages.SourceData;
using System;
using System.Collections.Generic;
using System.IO;
using File = System.IO.File;

namespace ParsingPackages.Statistics
{
    public class PackagesUsingStatistic
    {
        private string projectsDir { get; set; }
        private List<string[]> extensions { get; set; } = new List<string[]>() 
        {
            new string[]{ "pacakge.json" },
            new string[]{ "csproj", "packages.config" },
            new string[]{ "Dockerfile"}
        };
        private string[] groupNames { get; set; } = new string[] { "npm-packages", "NuGet-packages", "Dockerfiles" };

        public PackagesUsingStatistic(string projectsDir)
        {
            this.projectsDir = projectsDir;
        }
        public PackagesUsingStatistic(string projectsDir, List<string[]> ext, string[] groups) 
        {
            this.projectsDir = projectsDir;
            extensions = ext;
            groupNames = groups;
        }

        public List<GroupStatisticData> getPackagesStatistic()
        {
            List<GroupStatisticData> pakagesStatistic = new List<GroupStatisticData>();
            string[] projectDirectories= Directory.GetDirectories(projectsDir);
            foreach (string projectDirectory in projectDirectories)
            {
                List<string> packagesFiles = new PackagesFiles().getPackagesFiles(projectDirectory);
                Console.WriteLine($"Total files = {packagesFiles.Count}");

                List<GroupStatisticData> projectStatistics = getProjectPackagesStatistic(packagesFiles);
                pakagesStatistic = unionStatistics(pakagesStatistic, projectStatistics);
            }
            return pakagesStatistic;
        }

        private List<GroupStatisticData> unionStatistics(List<GroupStatisticData> pakagesStatistic, List<GroupStatisticData> projectStatistics)
        {
            if (pakagesStatistic == null || pakagesStatistic.Count == 0)
            {
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
                            ItemStatisticData item = null;
                            item = packagesStat.items.Find((item) => {
                                return item.data.Equals(projectItem.data);
                            });

                            if (item == null)
                            {
                                packagesStat.items.Add(new ItemStatisticData(projectItem.data, projectItem.total));
                            }
                            else
                            {
                                item.total += projectItem.total;
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
                    group.updateProjectStatistic(pkgFile);                     
                }
                pakagesStatistic.Add(group);
            }
            return pakagesStatistic;
        }

        public void show(List<GroupStatisticData> pakagesStatistic) 
        {
            writeToFile(Directory.GetCurrentDirectory() + "\\packages-statistics.txt", pakagesStatistic);

            Console.WriteLine(" - - - Pakages Statistic - - - \n");
            foreach (GroupStatisticData group in pakagesStatistic) 
            {
                string fileTypes = group.groupExtensions[0];
                for (int i = 1; i < group.groupExtensions.Length; i++)
                {
                    fileTypes+= " " + group.groupExtensions[i];
                }
                Console.WriteLine(" - " + group.groupName+ " (" + fileTypes + ")");
                if (group.items != null && group.items.Count > 0)
                {
                    group.items.Sort(delegate (ItemStatisticData x, ItemStatisticData y)
                    {
                        if (x.total == y.total)
                        {
                            return x.data.values[0].CompareTo(y.data.values[0]);
                        }
                        else
                        {
                            return x.total < y.total ? 1 : -1;
                        }
                        
                    });

                    for (int i = 0; i < group.items.Count; i++)
                    {
                        string packageData = group.items[i].data.values[0];
                        for (int j = 1; j < group.items[i].data.values.Length; j++)
                        {
                            packageData += " " + group.items[i].data.values[j];
                        }
                        Console.WriteLine($"{group.items[i].total, 5:G} used: " + packageData);
                    }
                }
                Console.WriteLine();
            }
        }

        public void writeToFile(string pathFile, List<GroupStatisticData> pakagesStatistic) 
        {
            using (StreamWriter file = File.CreateText(pathFile)) { 

            file.WriteLine(" - - - Pakages Statistic - - - \n");
            foreach (GroupStatisticData group in pakagesStatistic)
            {
                string fileTypes = group.groupExtensions[0];
                for (int i = 1; i < group.groupExtensions.Length; i++)
                {
                    fileTypes += " " + group.groupExtensions[i];
                }
                file.WriteLine(" - " + group.groupName + " (" + fileTypes + ")");
                if (group.items != null && group.items.Count > 0)
                {
                    group.items.Sort(delegate (ItemStatisticData x, ItemStatisticData y)
                    {
                        if (x.total == y.total)
                        {
                            return x.data.values[0].CompareTo(y.data.values[0]);
                        }
                        else
                        {
                            return x.total < y.total ? 1 : -1;
                        }

                    });

                    for (int i = 0; i < group.items.Count; i++)
                    {
                        string packageData = group.items[i].data.values[0];
                        for (int j = 1; j < group.items[i].data.values.Length; j++)
                        {
                            packageData += " " + group.items[i].data.values[j];
                        }
                        file.WriteLine($"{group.items[i].total,5:G} used: " + packageData);
                    }
                }
                file.WriteLine();
            }
            }
            Console.WriteLine($"Statistic data was wrote to '{pathFile}'");
        } 

    }
}
