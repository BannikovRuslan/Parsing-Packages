using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPackages.Statistics
{
    public class ItemStatisticData
    {
        /// <summary>
        /// Данные по пакету в виде { "значение атрибута 1", "значение атрибута 2", ...}
        /// </summary>
        public ItemData data { get; set; }
        public int total { get; set; }
        public ItemStatisticData(ItemData data, int total)
        {
            this.data = data;
            this.total = total;
        }   
    }
}
