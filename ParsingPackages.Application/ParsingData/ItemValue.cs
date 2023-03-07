using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingPackages.Application.ParsingData
{
    public class ItemValue
    {
        public string name { get; set; }
        public string value { get; set; }
        public ItemValue() { }
        public ItemValue(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
