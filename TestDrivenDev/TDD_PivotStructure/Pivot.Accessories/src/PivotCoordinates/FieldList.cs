using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.PivotCoordinates
{
    public class FieldList :  List<string>, IComparable
    {
        public int CompareTo(object obj)
        {
            var listObj = (List<string>)obj;

            if (listObj == null)
                throw new Exception("incorrect object type or null given for comparison");

            if (listObj.Count != this.Count)
                throw new Exception("list counts are different");

            int comparisonResult = 0;
            int factor = 1;
            for(int i=0; i<listObj.Count; i++)
            {
                comparisonResult += (string.IsNullOrEmpty(listObj[i]) ? -1 : listObj[i].CompareTo(this[i])) * factor;
                factor *= 10;
            }

            return comparisonResult;
        }

        //public override bool Equals(object obj)
        //{
        //    var listObj = (ListComparer)obj;
        //    for (int i = 0; i < listObj.Count; i++)
        //    {
        //        if (listObj[i] != this[i])
        //            return false;
        //    }
        //    return true;
        //}

        public int GetRank()
        {
            int rank = 0;

            foreach(string s in this)
            {
                if (string.IsNullOrEmpty(s))
                    continue;
                rank++;
            }

            return rank - 1;
        }
        public int GetReverseRank()
        {
            return (this.Count - 1) - GetRank();
        }
    }

    public class SortFieldListAscendingHelper<T> : IComparer<T> where T : IComparable
    {
        int IComparer<T>.Compare(T a, T b)
        {
            return a.CompareTo(b);
        }
    }


    public class FieldListComparer : IEqualityComparer<FieldList>
    {
        private static string _largestString;
        private string LargestString
        {
            get
            {
                if (_largestString == null)
                {
                    char[] buffer = new char[1000];
                    for (int i = 0; i < 1000; i++)
                    {
                        buffer[i] = 'Z';
                    }
                    _largestString = buffer.ToString();
                }
                return _largestString;
            }
        }

        public bool Equals(FieldList x, FieldList y)
        {
            if (x.Count != y.Count)
                throw new Exception("Lists of fields are Incomparable since their dimensions are not matching");

            for(int i = 0; i < x.Count; i++ )
            {
                if (x.ElementAt(i) != y.ElementAt(i))
                    return false;
            }
            return true;
        }

        public int GetHashCode(FieldList listComparer)
        {
            int accumulatedHash = 0;
            int multiplier = 80;

            foreach(var li in listComparer)
            {
                accumulatedHash += (string.IsNullOrEmpty(li) ? LargestString.GetHashCode() : li.GetHashCode()) * multiplier;
                multiplier--;
            }

            return accumulatedHash;
        }
    }

}
