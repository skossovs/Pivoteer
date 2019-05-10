using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pivot.Accessories.Mapping;
using TDD_PivotStructure.DataStructures;

namespace TDD_PivotStructure.Attributes
{
    [TestClass]
    public class TDD_FieldMapping
    {
        [TestMethod]
        public void Test_X_Mapper()
        {
            var testObject = new FourByFour();
            testObject.Ticker             = "HIMX";
            testObject.TradeVolumeBracket = "10..50";
            testObject.MarketCapBracket   = "100M..1B";
            testObject.PERatioBracket     = "0..20";

            var wrapper = new XTypeWrapper<FourByFour, AggregationFunctions>();

            Assert.AreEqual("HIMX"    , wrapper.GetField(testObject, 0));
            Assert.AreEqual("10..50"  , wrapper.GetField(testObject, 1));
            Assert.AreEqual("100M..1B", wrapper.GetField(testObject, 2));
            Assert.AreEqual("0..20"   , wrapper.GetField(testObject, 3));
        }

        [TestMethod]
        public void Test_Y_Mapper()
        {
            var testObject      = new FourByFour();
            testObject.Day      = "1";
            testObject.Month    = "May";
            testObject.Quarter  = "II";
            testObject.Year     = "2019";

            var wrapper = new YTypeWrapper<FourByFour, AggregationFunctions>();

            Assert.AreEqual("1"    , wrapper.GetField(testObject, 0));
            Assert.AreEqual("May"  , wrapper.GetField(testObject, 1));
            Assert.AreEqual("II"   , wrapper.GetField(testObject, 2));
            Assert.AreEqual("2019" , wrapper.GetField(testObject, 3));
        }

        [TestMethod]
        public void Test_Value_Mapper()
        {
            var testObject = new FourByFour();
            testObject.Price = 250.25m;

            var wrapper = new ValueTypeWrapper<FourByFour>();

            Assert.AreEqual(250.25m, wrapper.GetValue(testObject));
        }

        }
    }
