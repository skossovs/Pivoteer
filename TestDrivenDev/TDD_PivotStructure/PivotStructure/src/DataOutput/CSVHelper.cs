using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotStructure.DataOutput
{
    internal class CSVHelper
    {
        public static void SaveCSVFile(string[,] matrix, string fname)
        {
            using (var f = System.IO.File.CreateText(fname))
            {
                for (int i = 0; i < matrix.GetLength(1); i++)
                {
                    StringBuilder strOut = new StringBuilder();

                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        strOut.Append(matrix[j, i] + ',');
                    }
                    f.WriteLine(strOut);
                }
            }
        }

        public static void SaveDictionary<T>(Dictionary<T, int> dic, string fname)
        {
            using (var f = System.IO.File.CreateText(fname))
            {
                foreach (var kv in dic)
                {
                    var row = kv.Key.ToString() + "," + kv.Value;
                    f.WriteLine(row);
                }
            }

        }

        public static void SaveSortList<T>(SortedDictionary<T, int> dic, string fname)
        {
            using (var f = System.IO.File.CreateText(fname))
            {
                foreach (var kv in dic)
                {
                    var row = kv.Key.ToString() + "," + kv.Value;
                    f.WriteLine(row);
                }
            }

        }
    }
}
