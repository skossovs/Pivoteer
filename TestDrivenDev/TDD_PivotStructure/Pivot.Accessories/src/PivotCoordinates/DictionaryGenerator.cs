using Pivot.Accessories.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.PivotCoordinates
{
    public class DictionaryGenerator<T, TAggregator> where T : class where TAggregator : class
    {
        TypeWrapper<T, TAggregator> typeWrapper;
        public DictionaryGenerator(TypeWrapper<T, TAggregator> t)
        {
            typeWrapper = t;
        }

        public SortedDictionary<FieldList, int> GenerateXDictionary(IEnumerable<T> list)
        {
            var queryBuilder = new QueryBuilder<T, TAggregator>(typeWrapper);

            var queryElements = new List<IEnumerable<FieldList>>();
            for (int i = 0; i < typeWrapper.XType.MaxDim; i++)
            {
                var q0 = queryBuilder.XBuildOrderBy(list, i);
                var l = queryBuilder.XBuildSelect(q0, i).Distinct(new FieldListComparer());
                queryElements.Add(l);
            }

            IEnumerable<FieldList> q = null;
            bool isFirst = true;
            foreach(var t in queryElements)
            {
                if (isFirst)
                {
                    isFirst = false;
                    q = t.Distinct();
                }
                else
                    q = q.Union(t.Distinct());
            }

            var ieResult = q.Select(l => new { FieldList = l, Index = 0 });

            ieResult = ieResult.OrderBy(t => t.FieldList);

            int ix = typeWrapper.YType.MaxDim; // immediate shift
            var sortedX = new SortedDictionary<FieldList, int>(new SortFieldListAscendingHelper<FieldList>());
            ieResult.ToList().ForEach(t => sortedX.Add(t.FieldList, ix++));

            return sortedX;
        }

        public SortedDictionary<FieldList, int> GenerateYDictionary(IEnumerable<T> list)
        {
            var oneFold = new QueryBuilder<T, TAggregator>(typeWrapper);

            var queryElements = new List<IEnumerable<FieldList>>();
            for (int i = 0; i < typeWrapper.YType.MaxDim; i++)
            {
                var q0 = oneFold.YBuildOrderBy(list, i);
                var l = oneFold.YBuildSelect(q0, i).Distinct(new FieldListComparer());
                queryElements.Add(l);
            }

            IEnumerable<FieldList> q = null;
            bool isFirst = true;
            foreach (var t in queryElements)
            {
                if (isFirst)
                {
                    isFirst = false;
                    q = t.Distinct();
                }
                else
                    q = q.Union(t.Distinct());
            }

            var ieResult = q.Select(l => new { FieldList = l, Index = 0 });

            ieResult = ieResult.OrderBy(t => t.FieldList);

            int iy = typeWrapper.XType.MaxDim; // immediate shift
            var sortedY = new SortedDictionary<FieldList, int>();
            ieResult.ToList().ForEach(t => sortedY.Add(t.FieldList, iy++));

            return sortedY;
        }


    }
}
