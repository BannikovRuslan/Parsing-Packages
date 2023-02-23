using ParsingPackages.SourceData;
using ParsingPackages.Statistics;
using System;
using System.Collections.Generic;

namespace ParsingPackages
{
    internal class Program
    {
        const string path = "D:\\Work\\Projects\\ParsingPackages\\ParsingPackages\\TestData";
        static void Main(string[] args)
        {
            PackagesUsingStatistic packagesUsingStatistic = new PackagesUsingStatistic(path);
            List<GroupStatisticData> pakagesStatistic = packagesUsingStatistic.getPackagesStatistic();
            packagesUsingStatistic.show(pakagesStatistic);
            packagesUsingStatistic.writeToFile(path + "\\packages-statistic.txt", pakagesStatistic);
        }
    }

}
