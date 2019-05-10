using System;
using System.Collections.Generic;
using TDD_PivotStructure.DataStructures;

namespace TDD_PivotStructure.DataGenerators
{
    public class SmallDatasets
    {
        public static List<TwoByTwo> GenerateFullFlatData()
        {
            var table = new List<TwoByTwo>();

            foreach (var Y in new string[] { "2011", "2012" })
            {
                foreach (var M in new string[] {"Jan","Feb"})
                {
                    foreach (var QM in new Tuple<string, string>[] { new Tuple<string, string>("AK"  ,"1000"),
                                                                 new Tuple<string, string>("AK"  ,"1001"),
                                                                 new Tuple<string, string>("AL"  ,"2000"),
                                                                 new Tuple<string, string>("AL" ,"2001")})
                    {
                        table.Add(new TwoByTwo()
                        {
                            Year = Y,
                            Month = M,
                            State = QM.Item1,
                            Zip = QM.Item2,
                            Temperature = 100
                        });
                    }
                }
            }

            return table;
        }
    }
}
