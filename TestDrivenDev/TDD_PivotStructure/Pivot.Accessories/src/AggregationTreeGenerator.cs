using System;
using System.Collections.Generic;
using Pivot.Accessories;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pivot.Accessories.PivotCoordinates;
using Pivot.Accessories.Mapping;
using System.Linq.Expressions;
using Pivot.Accessories.Extensions;

namespace Pivot.Accessories
{
    public class AggregationTreeGenerator<T, TAggregator> where T : class where TAggregator : class
    {
        TypeWrapper<T, TAggregator> typeWrapper;
        private const int DecimalErrorNumber = -2146233033;

        public AggregationTreeGenerator(TypeWrapper<T, TAggregator> t)
        {
            typeWrapper = t;
        }

        public AggregationTreeNode GenerateXAggregationTree(SortedDictionary<FieldList, int> dicX)
        {
            // start with leaf nodes
            AggregationTreeNode attSeed = AggregationTreeNodeFactory.CreateBranch();
            attSeed.Level = typeWrapper.XType.MaxDim;
            var seedState = new State() { attUpper = attSeed, level = typeWrapper.XType.IndexMaxDim, dictionary = dicX };
            GenerateAggregationTreeRecursive(seedState, false);

            return attSeed;
        }

        public AggregationTreeNode GenerateYAggregationTree(SortedDictionary<FieldList, int> dicY)
        {
            // start with leaf nodes
            AggregationTreeNode attSeed = AggregationTreeNodeFactory.CreateBranch();
            attSeed.Level = typeWrapper.YType.MaxDim;

            var seedState = new State() { attUpper = attSeed, level = typeWrapper.YType.IndexMaxDim, dictionary = dicY };
            GenerateAggregationTreeRecursive(seedState, true);

            return attSeed;
        }

        private struct State
        {
            public AggregationTreeNode attUpper;
            public AggregationTreeNode previous;
            public SortedDictionary<FieldList, int> dictionary;
            public int level;
        }
        private AggregationTreeNode GenerateAggregationTreeRecursive(State state, bool isYFork)
        {
            IEnumerable<AggregationTreeNode> q;

            q = GenerateLevelTree(state.level, state.dictionary);

            if (isYFork)
            {
                if (typeWrapper.YType.IndexMaxDim != state.level)
                    q = FilterOutExtra(state.attUpper, state.previous, q);
            }
            else
            {
                if (typeWrapper.XType.IndexMaxDim != state.level)
                    q = FilterOutExtra(state.attUpper, state.previous, q);
            }

            AggregationTreeNode previousAggregationTreeNode = state.previous; // establish starting point from upper level

            foreach (var aggregationTreeNode in q)
            {
                state.attUpper.Children.Add(aggregationTreeNode);
                if (state.level > 0)
                {
                    var lowerState = new State() { attUpper = aggregationTreeNode, level = state.level - 1, dictionary = state.dictionary, previous = previousAggregationTreeNode };
                    GenerateAggregationTreeRecursive(lowerState, isYFork);
                }
                previousAggregationTreeNode = aggregationTreeNode;  // establish starting point from the sibling
            }

            return state.attUpper;
        }

        private static IEnumerable<AggregationTreeNode> FilterOutExtra(AggregationTreeNode upperNode, AggregationTreeNode previousNode, IEnumerable<AggregationTreeNode> q)
        {
            q = q.Where(t1 => t1.Dimmension < upperNode.Dimmension)
                 .Where(t1 => t1.Dimmension > (previousNode?.Dimmension ?? 0));
            return q;
        }

        #region functors

        public Func<int, decimal?> CreateXGetter(int y, string[,] matrix)
        {
            return  x => {
                try
                {
                    var v = matrix[x, y];
                    return string.IsNullOrEmpty(v) ? (decimal?)null : Convert.ToDecimal(v);
                }
                catch (Exception ex)
                {
                    if (ex.HResult == DecimalErrorNumber)
                        throw new MatrixGetterException(matrix, x, y);
                    else
                        throw;
                }
            };
        }
        public Action<int, decimal?> CreateXSetter(int y, string[,] matrix)
        {
            return (x, v) => matrix[x, y] = Convert.ToString(v);
        }
        public Func<int, decimal?> CreateYGetter(int x, string[,] matrix)
        {
            return y => {
                try
                {
                    var v = matrix[x, y];
                    return string.IsNullOrEmpty(v) ? (decimal?)null : Convert.ToDecimal(v);
                }
                catch(Exception ex)
                {
                    if (ex.HResult == DecimalErrorNumber)
                        throw new MatrixGetterException(matrix, x, y);
                    else
                        throw;
                }
            };
        }
        public Action<int, decimal?> CreateYSetter(int x, string[,] matrix)
        {
            return (y, v) => matrix[x, y] = Convert.ToString(v);
        }
        #endregion

        private IEnumerable<AggregationTreeNode> GenerateLevelTree(int level, SortedDictionary<FieldList, int> dic)
        {
            IEnumerable<AggregationTreeNode> q = dic
                .Where(s => s.Key.GetReverseRank() == level)
                .OrderBy(t => t.Key)
                .Select(a => new AggregationTreeNode
                {
                    Dimmension = a.Value,
                    Level = level,
                    Children = (level > 0) ? new List<AggregationTreeNode>() : null
                });

            foreach (var t in q)
                yield return t;
        }

    }
}
