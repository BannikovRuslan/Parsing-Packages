using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPackages.Statistics
{
    public class ItemStatisticData
    {
        /// <summary>
        /// Статистистические данные по пакету с набором считанных значений
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
