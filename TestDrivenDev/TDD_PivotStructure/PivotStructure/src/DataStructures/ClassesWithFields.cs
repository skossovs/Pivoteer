using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotStructure.DataStructures
{
    [Pivot.Accessories.Attributes.Aggregators(AggregatorType = typeof(AggregationFunctions))]
    public class TwoByTwo
    {
        [Pivot.Accessories.Attributes.DimmensionX(Level = 0, AggregationFuncName = "Sum")]
        public string Zip;
        [Pivot.Accessories.Attributes.DimmensionX(Level = 1, AggregationFuncName = "Count")]
        public string State;
        [Pivot.Accessories.Attributes.DimmensionY(Level = 0, AggregationFuncName = "Sum")]
        public string Month;
        [Pivot.Accessories.Attributes.DimmensionY(Level = 1, AggregationFuncName = "Count")]
        public string Year;

        // Value
        [Pivot.Accessories.Attributes.IsValues]
        public decimal? Temperature;
    }

    [Pivot.Accessories.Attributes.Aggregators(AggregatorType = typeof(AggregationFunctions))]
    public class FourByFour
    {
        // Y - Dimmensions
        [Pivot.Accessories.Attributes.DimmensionX(Level = 0, AggregationFuncName = "Sum")]
        public string Ticker;
        [Pivot.Accessories.Attributes.DimmensionX(Level = 1, AggregationFuncName = "Sum")]
        public string TradeVolumeBracket;
        [Pivot.Accessories.Attributes.DimmensionX(Level = 2, AggregationFuncName = "Sum")]
        public string MarketCapBracket;
        [Pivot.Accessories.Attributes.DimmensionX(Level = 3, AggregationFuncName = "Sum")]
        public string PERatioBracket;

        // X - Dimmensions
        [Pivot.Accessories.Attributes.DimmensionY(Level = 3, AggregationFuncName = "Sum")]
        public string Year;
        [Pivot.Accessories.Attributes.DimmensionY(Level = 2, AggregationFuncName = "Sum")]
        public string Quarter;
        [Pivot.Accessories.Attributes.DimmensionY(Level = 1, AggregationFuncName = "Sum")]
        public string Month;
        [Pivot.Accessories.Attributes.DimmensionY(Level = 0, AggregationFuncName = "Sum")]
        public string Day;

        // Value
        [Pivot.Accessories.Attributes.IsValues]
        public decimal? Price;

    }

    [Serializable]
    [Pivot.Accessories.Attributes.Aggregators(AggregatorType = typeof(AggregationFunctions))]
    public class ShopRiteSales
    {
        // Y - Dimmensions
        [Pivot.Accessories.Attributes.DimmensionX(Level = 0, AggregationFuncName = "Sum")]
        public string Merchandise;
        [Pivot.Accessories.Attributes.DimmensionX(Level = 1, AggregationFuncName = "Sum")]
        public string MerchandiseGroup;
        [Pivot.Accessories.Attributes.DimmensionX(Level = 2, AggregationFuncName = "Sum")]
        public string Zip;
        [Pivot.Accessories.Attributes.DimmensionX(Level = 3, AggregationFuncName = "Sum")]
        public string State;

        // X - Dimmensions
        [Pivot.Accessories.Attributes.DimmensionY(Level = 3, AggregationFuncName = "Sum")]
        public string Year;
        [Pivot.Accessories.Attributes.DimmensionY(Level = 2, AggregationFuncName = "Sum")]
        public string Quarter;
        [Pivot.Accessories.Attributes.DimmensionY(Level = 1, AggregationFuncName = "Sum")]
        public string Month;
        [Pivot.Accessories.Attributes.DimmensionY(Level = 0, AggregationFuncName = "Sum")]
        public string Day;

        // Value
        [Pivot.Accessories.Attributes.IsValues]
        public decimal? Quantity;

    }


}
