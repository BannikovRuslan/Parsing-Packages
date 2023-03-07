using ParsingPackages.Logging;
using ParsingPackages.Statistics;
using ParsingPackages.Utils;
using System.Text.Json;
using System.Xml.Linq;

public static class ParsersHelpers
{
    public static List<ItemData> xmlParser(string fileData, List<XmlParseData> parseData)
    {
        List<ItemData> packages = new List<ItemData>();
        XDocument doc;
        try
        {
            doc = XDocument.Parse(fileData);
        }
        catch (Exception ex)
        {
            string message = "<xmlParser> -> Получены некорректные данные для парсинга! Данные будут проигнорированны!\n"+
                             ex.ToString();
            new Logger().warningLogger(message, true, true);
            return packages;
        }

        var docDescendants = doc.Descendants();
       
        foreach (XmlParseData data in parseData)
        {
            string node = data.node;
            packages.AddRange(docDescendants
                                .Where(e => 
                                {
                                    bool isExistsAttr = false;
                                    for (int i = 0; i < data.attributes.Length; i++)
                                    {
                                        if (e.Attribute(data.attributes[i]) != null)
                                        {
                                            isExistsAttr = true;
                                            break;
                                        }
                                    }
                                    return e.Name == node && isExistsAttr;
                                 })
                                .Select(e =>
                                {
                                    ItemData attr_value = new ItemData(new string[data.attributes.Length]);
                                    for (int i = 0; i < data.attributes.Length; i++)
                                    {
                                        attr_value.values[i] = e.Attribute(data.attributes[i]) == null? "" : e.Attribute(data.attributes[i]).Value;
                                    }
                                    return attr_value;
                                }).Distinct().ToList());
        }

        return distinctPackages(packages);
    }

    /// <summary>
    /// Парсин данных по именам объектов с первого уровня вложенности.
    /// Если значение объект, то получаем список { имя, значение }, соотвуствующие искомому объекту.
    /// Если значение строка, то получаем { имя искомого объекта, значение}
    /// </summary>
    /// <param name="fileData">JSON в виде строки</param>
    /// <param name="parseData">список имён объектов для парсинга</param>
    /// <returns></returns>
    public static List<ItemData> jsonParser(string fileData, List<JsonParseData> parseData)
    {
        List<ItemData> packages = new List<ItemData>();
        JsonDocument doc;
        try
        {
            doc = JsonDocument.Parse(fileData);
        }
        catch (Exception ex)
        {
            string message = "<jsonParser> -> Получены некорректные данные для парсинга! Данные будут проигнорированны!\n"+
                             ex.ToString();
            new Logger().warningLogger(message, true, true);
            return packages;
        }
        

        
        foreach (var item in doc.RootElement.EnumerateObject())
        {
            foreach (JsonParseData jsonParseData in parseData)
            {
                if (item.Name == jsonParseData.section)
                {
                    if (item.Value.ValueKind.ToString() == "String")
                    {
                        packages.Add(new ItemData(new string[] { item.Name, item.Value.ToString() }));
                    }
                    if (item.Value.ValueKind.ToString() == "Object")
                    {
                        foreach (var prop in item.Value.EnumerateObject())
                        {
                            packages.Add(new ItemData(new string[] { prop.Name, prop.Value.ToString() }));
                        }
                    }
                }
            }        
        }
        return distinctPackages(packages);
    }

    public static List<ItemData> dockerfileParser(string fileData, List<DockerfileParseData> parseData)
    {
        List<ItemData> packages = new List<ItemData>();
        foreach (string str in fileData.Split("\n"))
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