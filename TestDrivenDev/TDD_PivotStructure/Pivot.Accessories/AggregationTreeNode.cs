using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories
{
    public class AggregationTreeNode
    {
        public int      Dimmension;
        public int      Level;
        // public decimal? Value;           // Value is always equal Aggregated Children  !!! WRONG
        public List<AggregationTreeNode> Children;
        public bool IsLeaf {  get { return (Children is null); } }
    }

    public class AggregationTreeNodeFactory
    {
        public static AggregationTreeNode CreateLeaf()
        {
            return new AggregationTreeNode();
        }

        public static AggregationTreeNode CreateLeaf(int dimmension, int level)
        {
            return new AggregationTreeNode() { Dimmension = dimmension, Level = level };
        }

        public static AggregationTreeNode CreateBranch()
        {
            var tn = new AggregationTreeNode();
            tn.Children = new List<AggregationTreeNode>();
            return tn;
        }

        public static AggregationTreeNode CreateBranch(int dimmension, int level)
        {
            var tn                  = new AggregationTreeNode();
            tn.Dimmension           = dimmension;
            tn.Level                = level;
            tn.Children             = new List<AggregationTreeNode>();
            return tn;
        }

    }
}
