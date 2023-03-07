using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPackages.Utils
{
    /// <summary>
    /// Класс с указанными параметрами для парсинга xml-файлов
    /// </summary>
    public class XmlParseData
    {
        /// <summary>
        /// узел из которого будут вычитываться данные
        /// </summary>
        public string node { get; set; }
        /// <summary>
        /// имя атрибута узла из которого будут вычитываться данные
        /// </summary>
        public string[] attributes { get; set; }
        public XmlParseData(string node, string[] attributes)
        {
            this.node = node;
            this.attributes = attributes;
        }
    }
}
