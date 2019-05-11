using System;
using System.Collections.Generic;
using Pivot.Accessories;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pivot.Accessories.PivotCoordinates;
using Pivot.Accessories.Mapping;
using System.Linq.Expressions;

namespace Pivot.Accessories
{
    public class AggregationTreeGenerator<T, TAggregator> where T : class where TAggregator : class
    {
        TypeWrapper<T, TAggregator> typeWrapper;
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

            if(isYFork)
            {
                q = GenerateYLevelTree(state.level, state.dictionary);
                if (typeWrapper.YType.IndexMaxDim != state.level)
                {
                    q = q.Where(t1 => t1.Dimmension < state.attUpper.Dimmension)
                         .Where(t1 => t1.Dimmension > (state.previous?.Dimmension ?? 0));
                }
            }
            else
            {
                q = GenerateXLevelTree(state.level, state.dictionary);
                if (typeWrapper.XType.IndexMaxDim != state.level)
                { // TODO: copy-paste
                    q = q.Where(t1 => t1.Dimmension < state.attUpper.Dimmension)
                         .Where(t1 => t1.Dimmension > (state.previous?.Dimmension ?? 0));
                }
            }

            AggregationTreeNode previousState = null;

            foreach (var at in q)
            {
                state.attUpper.Children.Add(at);
                if (state.level > 0)
                {
                    var lowerState = new State() { attUpper = at, level = state.level - 1, dictionary = state.dictionary, previous = previousState };
                    GenerateAggregationTreeRecursive(lowerState, isYFork);
                }
                previousState = at;  // Counterintuitive moment here... NEED TO PRESERVE PREVIOUS STATE ALL OVER THE LEVELS !!!!!!
            }

            return state.attUpper;
        }

        #region functors

        public Func<int, decimal?> CreateXGetter(int y, string[,] matrix)
        {
            return  x => {
                var v = matrix[x, y];
                return string.IsNullOrEmpty(v) ? (decimal?) null : Convert.ToDecimal(v);
            };
        }
        public Action<int, decimal?> CreateXSetter(int y, string[,] matrix)
        {
            return (x, v) => matrix[x, y] = Convert.ToString(v);
        }
        public Func<int, decimal?> CreateYGetter(int x, string[,] matrix)
        {
            return y => {
                var v = matrix[x, y];
                return string.IsNullOrEmpty(v) ? (decimal?)null : Convert.ToDecimal(v);
            };
        }
        public Action<int, decimal?> CreateYSetter(int x, string[,] matrix)
        {
            return (y, v) => matrix[x, y] = Convert.ToString(v);
        }
        #endregion

        // TODO: no need to have 2 different functions, they do the same
        private IEnumerable<AggregationTreeNode> GenerateYLevelTree(int level, SortedDictionary<FieldList, int> dicY)
        {
            IEnumerable<AggregationTreeNode> q = dicY
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
        private IEnumerable<AggregationTreeNode> GenerateXLevelTree(int level, SortedDictionary<FieldList, int> dicX)
        {
            IEnumerable<AggregationTreeNode> q = dicX
                .Where(s => s.Key.GetReverseRank() == level)
                .OrderBy(t => t.Key)
                .Select(a => new AggregationTreeNode {
                    Dimmension = a.Value,
                    Level = level,
                    Children = (level > 0) ? new List<AggregationTreeNode>() : null
                });

            foreach (var t in q)
                yield return t;
        }
    }
}
