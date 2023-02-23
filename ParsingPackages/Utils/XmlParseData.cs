using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPackages.Utils
{
    public class XmlParseData
    {
        public XmlParseData(string node, string[] attributes)
        {
            this.node = node;
            this.attributes = attributes;
        }
        public string node { get; set; }
        public string[] attributes { get; set; }
    }
}
