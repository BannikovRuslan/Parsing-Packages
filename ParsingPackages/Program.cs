using ParsingPackages.Persistence.AzureDevOps;
using ParsingPackages.Presentation;
using ParsingPackages.Statistics;
using System;
using System.Collections.Generic;

namespace ParsingPackages
{
    internal class Program
    {
        async static Task Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            string packageName = "";
            AccessConfig accessConfig = new AccessConfig();
            updateInitData(args, ref accessConfig, ref packageName, ref path);

            List<GroupStatisticData> pakagesStatistic = await new PackagesUsingStatistic().getGlobalStatistic(accessConfig, false);
            GlobalStatisticPresentation.show(pakagesStatistic);
            GlobalStatisticPresentation.writeToFile(path + "\\packages-statistic.txt", pakagesStatistic);

            //List<GroupStatisticData> pakagesStatistic = new PackagesUsingStatistic().getGlobalStatistic(path);
            //GlobalStatisticPresentation.show(pakagesStatistic);
            //GlobalStatisticPresentation.writeToFile(path + "\\packages-statistic.txt", pakagesStatistic);

            if (packageName.Length > 0) 
            {
                List<AzureDevOpsProject> projects = PackageStatistic.getUsingProjects(pakagesStatistic, packageName);
                PackageStatisticPresentation.show(packageName, projects);
            }            
        }

        public static void updateInitData(string[] args, ref AccessConfig accessConfig, ref string package, ref string path)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "/user":
                        i += 1;
                        if (i < args.Length)
                        {
                            accessConfig.userName = args[i];
                        }
                        break;
                    case "/token":
                        i += 1;
                        if (i < args.Length)
                        {
                            accessConfig.accessToken = args[i];
                        }
                        break;
                    case "/server":
                        i += 1;
                        if (i < args.Length)
                        {
                            accessConfig.server = args[i];
                        }
                        break;
                    case "/collection":
                        i += 1;
                        if (i < args.Length)
                        {
                            accessConfig.collection = args[i];
                        }
                        break;
                    case "/package":
                        i += 1;
                        if (i < args.Length)
                        {
                            package = args[i];
                        }
                        break;
                    case "/path":
                        i += 1;
                        if (i < args.Length)
                        {
                            path = args[i];
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }

}
