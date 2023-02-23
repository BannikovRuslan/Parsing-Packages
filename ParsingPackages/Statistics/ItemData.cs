using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ParsingPackages.Statistics
{
    public class ItemData: IEquatable<ItemData>
    {
        public string[] values { get; set; }

        public ItemData(string[] values)
        {
            this.values = values;
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


    public class ItemDataComparer : IEqualityComparer<ItemData>
    {
        public bool Equals(ItemData x, ItemData y)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(x, null)) return false;
            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            bool equivalent = true;
            for (int i = 0; i < x.values.Length; i++)
            {
                if (x.values[i] != y.values[i])
                {
                    equivalent = false;
                    break;
                }
            }
            return equivalent;
        }

        public int GetHashCode(ItemData obj)
        {
            int hashProductCode = obj.values.GetHashCode();
            return hashProductCode;
        }
    }

}
