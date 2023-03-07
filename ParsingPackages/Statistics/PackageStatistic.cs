
using ParsingPackages.Persistence.AzureDevOps;
using System.Collections.Generic;

namespace ParsingPackages.Statistics
{
    public class PackageStatistic
    {
        public static List<AzureDevOpsProject> getUsingProjects(List<GroupStatisticData> pakagesStatistic, string packageName) 
        {
            List <AzureDevOpsProject> projects = new List<AzureDevOpsProject>();
            foreach (GroupStatisticData groupStat in pakagesStatistic)
            {
                foreach (ItemStatisticData itemStat in groupStat.items)
                {
                    if (itemStat.data.values.Contains(packageName))
                    {
                        foreach (string projectName in itemStat.data.projects)
                        {
                            projects.Add(new AzureDevOpsProject(projectName));
                        }
                    }
                }
            }
            return projects;
        }
    }
}
