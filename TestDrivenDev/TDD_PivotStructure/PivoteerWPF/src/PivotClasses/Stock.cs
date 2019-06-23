using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.PivotClasses
{
    class Stock : PivoteerWPF.MVVM.PivotClassBase
    {
        public string Sector;
        public string Industry;
        public string Company;
        public string Year;
        public string Quarter;
        public string Month;
        public string Day;

        public Decimal? Value;
    }
}
