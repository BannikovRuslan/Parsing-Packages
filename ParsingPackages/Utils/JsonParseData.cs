using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPackages.Utils
{
    public class JsonParseData
    {
        public string section { get; set; }
        public JsonParseData(string section) 
        {
            this.section = section;   
        }
    }
}
