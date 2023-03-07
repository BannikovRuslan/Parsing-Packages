using ParsingPackages.Persistence.AzureDevOps;
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

            //AccessConfig ac = new AccessConfig(path + "\\accessconfig.txt");
            //List<GroupStatisticData> st = await new PackagesUsingStatistic().getGlobalStatistic(ac, false);
            //PackagesUsingStatistic packagesUsingStatistic = new PackagesUsingStatistic();
            //packagesUsingStatistic.show(st);
            //packagesUsingStatistic.writeToFile(path + "\\packages-statistic.txt", st);

            PackagesUsingStatistic packagesUsingStatistic = new PackagesUsingStatistic();
            List<GroupStatisticData> pakagesStatistic = packagesUsingStatistic.getGlobalStatistic(path);
            packagesUsingStatistic.show(pakagesStatistic);
            packagesUsingStatistic.writeToFile(path + "\\packages-statistic.txt", pakagesStatistic);
        }
    }

}
