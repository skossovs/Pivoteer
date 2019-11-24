using Pivot.Accessories.Attributes;
using PivoteerWPF.Common;
using PivoteerWPF.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.PivotClasses
{
    [Aggregators(AggregatorType = typeof(AggregationFunctions))]
    public class TwoByTwo : PivotClassBase
    {
        // Y - Dimmensions
        [DimmensionY(Level = 1, AggregationFuncName = "Sum")]
        public string F00;
        [DimmensionY(Level = 0, AggregationFuncName = "Sum")]
        public string F01;

        // X - Dimmensions
        [DimmensionX(Level = 1, AggregationFuncName = "Sum")]
        public string F10;
        [DimmensionX(Level = 0, AggregationFuncName = "Sum")]
        public string F11;

        [IsValues]
        public Decimal? Value;
    }
}
