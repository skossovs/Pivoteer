using Pivot.Accessories.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.PivotCoordinates
{
    // This class build entites different on each step. Can be used only once.
    public class QueryBuilder<T, TAggregator> where T: class where TAggregator : class
    {
        private TypeWrapper<T, TAggregator> TypeWrapper;
        private int _maxXDim;
        private int _maxYDim;
        public QueryBuilder(TypeWrapper<T, TAggregator> typeWrapper)
        {
            TypeWrapper = typeWrapper;
            _maxXDim = typeWrapper.XType.MaxDim;
            _maxYDim = typeWrapper.YType.MaxDim;
        }
        public IEnumerable<T> XBuildOrderBy(IEnumerable<T> list, int level)
        {
            Func<T, int, string> getterFunc = (obj, j) => TypeWrapper.XType.GetField(obj, j);
            var q = list;
            for (int i = level; i < _maxXDim; i++)
            {
                var non_closure_index = i;
                q = q.OrderBy(l => getterFunc(l, non_closure_index));
            }
            return q;
        }

        public IEnumerable<FieldList> XBuildSelect(IEnumerable<T> list, int level)
        {
            Func<T, int, string> getterFunc = (obj, j) => TypeWrapper.XType.GetField(obj, j);
            var q = list.Select((t) => {
                var lst = new FieldList();

                for (int xi = 0; xi < _maxXDim; xi++)
                {
                    var nonClosureIndex = xi;
                    if(xi >= level)
                        lst.Add(getterFunc(t, nonClosureIndex));
                    else
                        lst.Add(string.Empty);
                }
                return lst;
            }).Distinct();

            return q;
        }

        public IEnumerable<T> YBuildOrderBy(IEnumerable<T> list, int level)
        {
            Func<T, int, string> getterFunc = (obj, j) => TypeWrapper.YType.GetField(obj, j);
            var q = list;
            for (int i = level; i < _maxYDim; i++)
            {
                var non_closure_index = i;
                q = q.OrderBy(l => getterFunc(l, non_closure_index));
            }
            return q;
        }

        public IEnumerable<FieldList> YBuildSelect(IEnumerable<T> list, int level)
        {
            Func<T, int, string> getterFunc = (obj, j) => TypeWrapper.YType.GetField(obj, j);
            var q = list.Select((t) => {
                var lst = new FieldList();

                for (int yi = 0; yi < _maxYDim; yi++)
                {
                    var nonClosureIndex = yi;
                    if (yi >= level)
                        lst.Add(getterFunc(t, nonClosureIndex));
                    else
                        lst.Add(string.Empty);
                }
                return lst;
            });

            return q;
        }


    }
}
