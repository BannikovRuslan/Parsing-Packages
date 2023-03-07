using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPackages.Utils
{
    /// <summary>
    /// Класс с указанными параметрами для парсинга JSON-файлов
    /// </summary>
    public class JsonParseData
    {
        /// <summary>
        /// имя секции из которой будут вычитываться данные
        /// </summary>
        public string section { get; set; }
        public JsonParseData(string section) 
        {
            this.section = section;   
        }
    }
}
