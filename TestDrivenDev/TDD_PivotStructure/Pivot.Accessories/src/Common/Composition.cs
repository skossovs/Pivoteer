using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.Common
{
    public static class Composition
    {
        public static Func<T2> Compose<T1, T2>(Func<T1> f1, Func<T1, T2> f2)
        {
            return () => f2(f1());
        }

        public static Func<T1, T3> Compose<T1, T2, T3>(Func<T1, T2> f1, Func<T2, T3> f2)
        {
            return v => f2(f1(v));
        }

        // example : Func<IEnumerable<double>, int> => Func<IEnumerable<double>, double>
        public static Func<IEnumerable<T2>, T2> Compose<T1, T2>(Func<IEnumerable<T2>, T1> f1, Func<T1, T2> f2)
        {
            return (ie) => f2(f1(ie));
        }

    }
}
