using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPackages.Utils
{
    /// <summary>
    /// Класс с указанными параметрами для парсинга Dockerfile
    /// </summary>
    public class DockerfileParseData
    {
        /// <summary>
        /// имя инструкции из которой будут вычитываться данные 
        /// </summary>
        public string instruction { get; set; }
        public DockerfileParseData(string instruction)
        {
            this.instruction = instruction;
        }   
    }
}
