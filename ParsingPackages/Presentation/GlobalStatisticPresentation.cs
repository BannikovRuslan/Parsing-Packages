using ParsingPackages.Statistics;

namespace ParsingPackages.Presentation
{
    public class GlobalStatisticPresentation
    {
        public static void show(List<GroupStatisticData> pakagesStatistic)
        {
            Console.WriteLine(" - - - Pakages Statistic - - - \n");
            foreach (GroupStatisticData group in pakagesStatistic)
            {
                string fileTypes = group.groupExtensions[0];
                for (int i = 1; i < group.groupExtensions.Length; i++)
                {
                    fileTypes += " " + group.groupExtensions[i];
                }
                Console.WriteLine(" - " + group.groupName + " (" + fileTypes + ")");
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
                        packageData += "\n\t in projects:\n";
                        foreach (string projectName in group.items[i].data.projects)
                        {
                            packageData += $"\t\t" + projectName + "\n";
                        }
                        Console.WriteLine($"{group.items[i].total,5:G} used: " + packageData);
                    }
                }
                Console.WriteLine();
            }
        }

        public static void writeToFile(string pathFile, List<GroupStatisticData> pakagesStatistic)
        {
            using (StreamWriter file = File.CreateText(pathFile))
            {

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
                            packageData += "\n\t in projects:\n";
                            foreach (string projectName in group.items[i].data.projects)
                            {
                                packageData += $"\t\t" + projectName + "\n";
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
