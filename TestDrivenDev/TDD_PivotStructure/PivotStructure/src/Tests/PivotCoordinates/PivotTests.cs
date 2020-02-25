using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pivot.Accessories.PivotCoordinates;
using PivotStructure.DataGenerators;
using PivotStructure.DataStructures;
using Pivot.Accessories;

namespace PivotStructure.PivotCoordinates
{
    /// <summary>
    /// Summary description for PivotTests
    /// </summary>
    [TestClass]
    public class PivotTests
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestPivotGenerator()
        {
            var data = ShopRiteSalesData.GenerateFullFlatData();

            var typeWrapper = new Pivot.Accessories.Mapping.TypeWrapper<ShopRiteSales, AggregationFunctions>();
            var generator = new PivotGenerator<ShopRiteSales, AggregationFunctions>(typeWrapper);

            var mtx = generator.GeneratePivot(data).Matrix;
            DataOutput.CSVHelper.SaveCSVFile(mtx, "ShopRitesSalesPivot.csv");
        }

        [TestMethod]
        public void TestSmallPivotGenerator()
        {
            var data = SmallDatasets.GenerateFullFlatData();

            var typeWrapper = new Pivot.Accessories.Mapping.TypeWrapper<TwoByTwo, AggregationFunctions>();
            var generator = new PivotGenerator<TwoByTwo, AggregationFunctions>(typeWrapper);

            var mtx = generator.GeneratePivot(data).Matrix;
            DataOutput.CSVHelper.SaveCSVFile(mtx, "SmallTwoByTwoMatrix.csv");

        }


        [TestMethod]
        public void TestGenerateXLevelTree()
        {
            var data = SmallDatasets.GenerateFullFlatData();
            var typeWrapper = new Pivot.Accessories.Mapping.TypeWrapper<TwoByTwo, AggregationFunctions>();
            var generator = new DictionaryGenerator<TwoByTwo, AggregationFunctions>(typeWrapper);
            SortedDictionary<FieldList, int> dirX = generator.GenerateXDictionary(data);

            int level = 0;
            IEnumerable<AggregationTreeNode> q0 = GenerateAggregationTreeXAnyLevel(dirX, level);
            var lst = q0.ToList();
            Assert.AreEqual(4, lst.Count);

            level = 1;
            IEnumerable<AggregationTreeNode> q1 = GenerateAggregationTreeXAnyLevel(dirX, level);
            lst = q1.ToList();
            Assert.AreEqual(2, lst.Count);

        }

        private static IEnumerable<AggregationTreeNode> GenerateAggregationTreeXAnyLevel(SortedDictionary<FieldList, int> dirX, int level)
        {
            var lst = dirX.Select(s => new { s, rank = s.Key.GetReverseRank() }).ToList();

            return dirX
                .Where(s => s.Key.GetReverseRank() == level)
                .OrderBy(t => t.Key)
                .Select(a => new AggregationTreeNode
                {
                    Dimmension = a.Value,
                    Level = level,
                    Children = (level > 0) ? new List<AggregationTreeNode>() : null
                });
        }
    }
}
