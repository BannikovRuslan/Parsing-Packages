using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPackages.Utils
{
    public class DockerfileParseData
    {
        public string instruction { get; set; }
        public DockerfileParseData(string instruction)
        {
            this.instruction = instruction;
        }   
    }
}
