using Pivot.Accessories.Attributes;
using PivoteerWPF.Common;
using PivoteerWPF.MVVM;
using System;

namespace PivoteerWPF.PivotClasses
{
    [Aggregators(AggregatorType = typeof(AggregationFunctions))]
    class Stock : PivotClassBase
    {
        [DimmensionX(Level = 2, AggregationFuncName = "Sum")]
        public string Sector;
        [DimmensionX(Level = 1, AggregationFuncName = "Sum")]
        public string Industry;
        [DimmensionX(Level = 0, AggregationFuncName = "Sum")]
        public string Company;
        [DimmensionX(Level = 3, AggregationFuncName = "Sum")]
        public string Year;
        [DimmensionX(Level = 2, AggregationFuncName = "Sum")]
        public string Quarter;
        [DimmensionX(Level = 1, AggregationFuncName = "Sum")]
        public string Month;
        [DimmensionX(Level = 0, AggregationFuncName = "Sum")]
        public string Day;

        [IsValues]
        public Decimal? Value;
    }
}
