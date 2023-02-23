using ParsingPackages.Statistics;
using ParsingPackages.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;

public static class FileParsersHelpers
{
    public static List<ItemData> xmlParser(string filePath, List<XmlParseData> parseData)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        XDocument doc = XDocument.Load(filePath);
        var docDescendants = doc.Descendants();

        List<ItemData> packages = new List<ItemData>();
        foreach (XmlParseData data in parseData)
        {
            string node = data.node;
            packages.AddRange(docDescendants
                                .Where(e => e.Name == node)
                                .Select(e =>
                                {
                                    ItemData attr_value = new ItemData(new string[data.attributes.Length]);
                                    for (int i = 0; i < data.attributes.Length; i++)
                                    {
                                        attr_value.values[i] = e.Attribute(data.attributes[i])?.Value;
                                    }
                                    return attr_value;
                                }).Distinct().ToList());
        }

        return distinctPackages(packages);
    }

    public static List<ItemData> jsonParser(string filePath, List<JsonParseData> parseData)
    {
        JsonDocument doc = JsonDocument.Parse(File.ReadAllText(filePath));
        var rootElement = doc.RootElement.EnumerateObject();

        List<ItemData> packages = new List<ItemData>();
        foreach (var item in rootElement)
        {
            foreach (JsonParseData jsonParseData in parseData)
            {
                if (item.Name == jsonParseData.section)
                {
                    foreach (var prop in item.Value.EnumerateObject())
                    {
                        packages.Add(new ItemData(new string[] { prop.Name }));
                    }
                }
            }        
        }
        return distinctPackages(packages);
    }

    public static List<ItemData> dockerfileParser(string filePath, List<DockerfileParseData> parseData)
    {
        var doc = File.ReadAllLines(filePath);
       
        List<ItemData> packages = new List<ItemData>();
        foreach (string str in doc)
        {
            foreach (DockerfileParseData dockerfileParseData in parseData)
            {
                if (str.Contains(dockerfileParseData.instruction))
                {
                    string[] words = str.Split();
                    packages.Add(new ItemData(new string[] { words[1] }));
                }
            }
        }
        
        return distinctPackages(packages);
    }

    public static List<ItemData> distinctPackages(List<ItemData> packages)
    {
        List<ItemData> resultPackages = new List<ItemData>();
        foreach (ItemData item in packages)
        {
            bool isExist = false;
            foreach (ItemData resultItem in resultPackages)
            {
                if (item.Equals(resultItem))
                {
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
            {
                resultPackages.Add(item);
            }
        }
        return resultPackages;
    }
}