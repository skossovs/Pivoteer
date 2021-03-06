﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pivot.Accessories.PivotCoordinates;
using System.Linq;
using PivotStructure.DataGenerators;
using PivotStructure.DataStructures;


namespace PivotStructure.PivotCoordinates
{
    [TestClass]
    public class DictionaryTests
    {
        [TestMethod]
        public void TestXDictionary()
        {
            var data = ShopRiteSalesData.GenerateXMetricsData();

            var typeWrapper = new Pivot.Accessories.Mapping.TypeWrapper<ShopRiteSales, AggregationFunctions>();
            var generator = new DictionaryGenerator<ShopRiteSales, AggregationFunctions>(typeWrapper);

            var dirX = generator.GenerateXDictionary(data);

            Assert.AreEqual(65, dirX.Count); // expecting exact amount for combinations
        }

        [TestMethod]
        public void TestYDictionary()
        {
            var data = ShopRiteSalesData.GenerateYMetricsData();

            var typeWrapper = new Pivot.Accessories.Mapping.TypeWrapper<ShopRiteSales, AggregationFunctions>();
            var generator = new DictionaryGenerator<ShopRiteSales, AggregationFunctions>(typeWrapper);

            var dirY = generator.GenerateYDictionary(data);

            Assert.AreEqual(51, dirY.Count); // expecting exact amount for combinations
        }

        [TestMethod]
        public void TestBuildOrderBy()
        {
            var data = ShopRiteSalesData.GenerateXMetricsData();
            var typeWrapper = new Pivot.Accessories.Mapping.TypeWrapper<ShopRiteSales, AggregationFunctions>();

            var queryBuilderObj = new QueryBuilder<ShopRiteSales, AggregationFunctions>(typeWrapper);

            var result0 = queryBuilderObj.XBuildOrderBy(data, 0);
            Assert.IsNotNull(result0.ElementAt(14));

            var result1 = queryBuilderObj.XBuildOrderBy(data, 1);
            Assert.IsNotNull(result0.ElementAt(14));

            var result2 = queryBuilderObj.XBuildOrderBy(data, 2);
            Assert.IsNotNull(result0.ElementAt(14));

            var result3 = queryBuilderObj.XBuildOrderBy(data, 3);
            Assert.IsNotNull(result0.ElementAt(14));

        }


        [TestMethod]
        public void TestXYDictionary()
        {
            var data = ShopRiteSalesData.GenerateFullFlatData();

            var typeWrapper = new Pivot.Accessories.Mapping.TypeWrapper<ShopRiteSales, AggregationFunctions>();
            var generator = new DictionaryGenerator<ShopRiteSales, AggregationFunctions>(typeWrapper);

            // checking X
            var dirX = generator.GenerateXDictionary(data);
            Assert.AreEqual(82, dirX.Count); // expecting exact amount for combinations

            var dirY = generator.GenerateYDictionary(data);
            Assert.AreEqual(411, dirY.Count); // expecting exact amount for combinations
        }

        [TestMethod]
        public void TestXYDictionarySmallSet_CheckOrder()
        {
            var data = SmallDatasets.GenerateFullFlatData();

            var typeWrapper = new Pivot.Accessories.Mapping.TypeWrapper<TwoByTwo, AggregationFunctions>();
            var generator = new DictionaryGenerator<TwoByTwo, AggregationFunctions>(typeWrapper);

            // checking X
            var dirX = generator.GenerateXDictionary(data);
            Assert.AreEqual(6, dirX.Count); // expecting exact amount for combinations

        }
    }
}
