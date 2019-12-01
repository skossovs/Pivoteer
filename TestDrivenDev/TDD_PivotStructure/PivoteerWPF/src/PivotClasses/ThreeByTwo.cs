using Pivot.Accessories.Attributes;
using PivoteerWPF.Common;
using PivoteerWPF.MVVM;
using System;

namespace PivoteerWPF.PivotClasses
{
    [Aggregators(AggregatorType = typeof(AggregationFunctions))]
    public class ThreeByTwo : PivotClassBase
    {
        // Y - Dimmensions
        [DimmensionY(Level = 2, AggregationFuncName = "Sum")]
        public string F00;
        [DimmensionY(Level = 1, AggregationFuncName = "Sum")]
        public string F01;
        [DimmensionX(Level = 0, AggregationFuncName = "Sum")]
        public string F10;

        // X - Dimmensions
        [DimmensionX(Level = 1, AggregationFuncName = "Sum")]
        public string F11;
        [DimmensionX(Level = 0, AggregationFuncName = "Sum")]
        public string F12;

        [IsValues]
        public Decimal? Value;
    }
}
