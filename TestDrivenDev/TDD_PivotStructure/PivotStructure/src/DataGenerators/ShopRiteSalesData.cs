using Pivot.Accessories.Common;
using System;
using System.Collections.Generic;
using PivotStructure.DataStructures;

namespace PivotStructure.DataGenerators
{
    public class ShopRiteSalesData
    {
        public static List<ShopRiteSales> GenerateXMetricsData()
        {
            var table =          new List<ShopRiteSales>();
            var changing_table = new List<ShopRiteSales>();

            changing_table.Add(new ShopRiteSales() { State = "NY", Zip = "10001", MerchandiseGroup = "Beverages", Merchandise = "Earl Grey Tea"   });
            changing_table.Add(new ShopRiteSales() { State = "NY", Zip = "10001", MerchandiseGroup = "Beverages", Merchandise = "Sumatran Coffee" });
            changing_table.Add(new ShopRiteSales() { State = "NY", Zip = "10001", MerchandiseGroup = "Beverages", Merchandise = "Earl Green Tea"  });
            changing_table.Add(new ShopRiteSales() { State = "NY", Zip = "10001", MerchandiseGroup = "Beverages", Merchandise = "Mint Tea"        });

            changing_table.Add(new ShopRiteSales() { State = "NY", Zip = "10001", MerchandiseGroup = "Sea Food" , Merchandise  = "Calamari"        });
            changing_table.Add(new ShopRiteSales() { State = "NY", Zip = "10001", MerchandiseGroup = "Sea Food" , Merchandise  = "Salmon"          });
            changing_table.Add(new ShopRiteSales() { State = "NY", Zip = "10001", MerchandiseGroup = "Sea Food" , Merchandise  = "Eel"             });

            changing_table.Add(new ShopRiteSales() { State = "NY", Zip = "10001", MerchandiseGroup = "Diary"    , Merchandise = "Milk"             });
            changing_table.Add(new ShopRiteSales() { State = "NY", Zip = "10001", MerchandiseGroup = "Diary"    , Merchandise = "Sour Cream"       });
            changing_table.Add(new ShopRiteSales() { State = "NY", Zip = "10001", MerchandiseGroup = "Diary"    , Merchandise = "Pudding"          });
            changing_table.Add(new ShopRiteSales() { State = "NY", Zip = "10001", MerchandiseGroup = "Diary"    , Merchandise = "Home Cheese"      });
            changing_table.Add(new ShopRiteSales() { State = "NY", Zip = "10001", MerchandiseGroup = "Diary"    , Merchandise = "Yougurt"          });

            table.AddRange(changing_table.Clone());
            changing_table.ForEach(li => li.Zip = "10305");

            table.AddRange(changing_table.Clone());
            changing_table.ForEach(li => li.Zip = "10314");

            table.AddRange(changing_table.Clone());
            changing_table.ForEach(li => li.Zip = "10318");
            table.AddRange(changing_table.Clone());

            return table;
        }
        public static List<ShopRiteSales> GenerateYMetricsData()
        {
            var table = new List<ShopRiteSales>();
            var changing_table = new List<ShopRiteSales>();

            changing_table.Add(new ShopRiteSales() { Year = "2011", Quarter = "Q1", Month = "Jan", Day = "1" });
            changing_table.Add(new ShopRiteSales() { Year = "2011", Quarter = "Q1", Month = "Jan", Day = "2" });
            changing_table.Add(new ShopRiteSales() { Year = "2011", Quarter = "Q1", Month = "Jan", Day = "3" });
            changing_table.Add(new ShopRiteSales() { Year = "2011", Quarter = "Q1", Month = "Jan", Day = "4" });

            table.AddRange(changing_table.Clone());
            changing_table.ForEach(li => li.Month = "Feb");

            table.AddRange(changing_table.Clone());
            changing_table.ForEach(li => li.Month = "Mar");

            table.AddRange(changing_table.Clone());

            // new years:
            changing_table = new List<ShopRiteSales>();
            changing_table.AddRange(table.Clone());
            changing_table.ForEach(li => li.Year = "2012");

            table.AddRange(changing_table.Clone());

            changing_table.ForEach(li => li.Year = "2013");
            table.AddRange(changing_table.Clone());

            return table;
        }
        public static List<ShopRiteSales> GenerateFullFlatData()
        {
            var table = new List<ShopRiteSales>();

            foreach(var Y in new string[] { "2011", "2012", "2013"})
            {
                foreach(var QM in new Tuple<string, string> [] { new Tuple<string, string>("I"  ,"Jan"),
                                                                 new Tuple<string, string>("I"  ,"Feb"),
                                                                 new Tuple<string, string>("I"  ,"Mar"),
                                                                 new Tuple<string, string>("II" ,"Apr"),
                                                                 new Tuple<string, string>("II" ,"May"),
                                                                 new Tuple<string, string>("II" ,"Jun"),
                                                                 new Tuple<string, string>("III","Jul"),
                                                                 new Tuple<string, string>("III","Aug"),
                                                                 new Tuple<string, string>("III","Sep"),
                                                                 new Tuple<string, string>("IV" ,"Oct"),
                                                                 new Tuple<string, string>("IV" ,"Nov"),
                                                                 new Tuple<string, string>("IV" ,"Dec")})
                {
                    foreach (var D in new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" })
                    {
                        foreach(var state in new string[] { "NY", "NJ"})
                        {
                            foreach(var zip in new string[] { "10001", "10002", "1003", "1004"})
                            {
                                foreach (var merchendize in new Tuple<string, string>[] { new Tuple<string, string>("Beverages", "Earl Grey Tea"),
                                                                                          new Tuple<string, string>("Beverages", "Sumatran Coffee"),
                                                                                          new Tuple<string, string>("Beverages", "Earl Green Tea"),
                                                                                          new Tuple<string, string>("Beverages", "Mint Tea"),
                                                                                          new Tuple<string, string>("Sea Food" , "Calamari"),
                                                                                          new Tuple<string, string>("Sea Food" , "Salmon"),
                                                                                          new Tuple<string, string>("Sea Food" , "Eel"),
                                })
                                {
                                    table.Add(new ShopRiteSales()   {
                                        Year             = Y,
                                        Quarter          = QM.Item1,
                                        Month            = QM.Item2,
                                        Day              = D,
                                        State            = state,
                                        Zip              = zip,
                                        MerchandiseGroup = merchendize.Item1,
                                        Merchandise      = merchendize.Item2,
                                        Quantity         = 10 });
                                }
                            }
                        }
                    }
                }
            }

            return table;
        }
    }
}
