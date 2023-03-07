using ParsingPackages.Persistence.AzureDevOps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingPackages.Presentation
{
    public class PackageStatisticPresentation
    {
        public static void show(string packageName, List<AzureDevOpsProject> projects)
        {
            Console.WriteLine($"[{packageName}] используется в проектах:");
            foreach (AzureDevOpsProject project in projects)
            {
                Console.WriteLine($"\t{project.projectId}");
            }
        }
    }
}
