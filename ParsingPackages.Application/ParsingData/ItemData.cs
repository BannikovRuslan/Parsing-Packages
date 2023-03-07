using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ParsingPackages.Statistics
{
    /// <summary>
    /// Класс для хранения набора считанных значений
    /// </summary>
    public class ItemData: IEquatable<ItemData>
    {
        public List<string> projects { get; set; } = new List<string>();
        public string[] values { get; set; }

        public ItemData(string[] values)
        {
            this.values = values;
        }
        public ItemData(string[] values, List<string> projects)
        {
            this.values = values;
            this.projects = projects;
        }      
        public bool Equals([AllowNull] ItemData other)
        {

            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;
            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            bool equivalent = true;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != other.values[i])
                {
                    equivalent = false;
                    break;
                }
            }
            return equivalent;
        }

        public override int GetHashCode()
        {
            //Get hash code for the Code field.
            int hashProductCode = values.GetHashCode();
            //Calculate the hash code for the product.
            return hashProductCode;
        }
    }

}
