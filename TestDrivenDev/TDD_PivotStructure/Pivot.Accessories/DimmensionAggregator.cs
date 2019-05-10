using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories
{
    /// <summary>
    /// Incapsulates the edge of a pivot and inside digital data
    /// </summary>
    public class DimmensionAggregator
    {
        private AggregationTreeNode _seedAggregationTree;

        private class TransmissionState
        {
            public List<decimal?> LevelValues;
            public List<AggregationTreeNode> TreeNodes;

            public TransmissionState()
            {
                LevelValues = new List<decimal?>();
                TreeNodes = new List<AggregationTreeNode>();
            }
        }

        public DimmensionAggregator(AggregationTreeNode seedAggregationTree)
        {
            _seedAggregationTree = seedAggregationTree;
        }

        public void DrillDownTree(MatrixManipulator manipulator)
        {
            DrillDownBranchRecursive(_seedAggregationTree, manipulator);
        }

        private TransmissionState DrillDownBranchRecursive(AggregationTreeNode tnUpper, MatrixManipulator manipulator)
        {
            var stateUpperDesignated = new TransmissionState();

            if (!tnUpper.IsLeaf)
            {
                foreach(var tn in tnUpper.Children)
                {
                    decimal? aggValue;
                    var aggState = DrillDownBranchRecursive(tn, manipulator);
                    aggValue = manipulator.AggregationFunctionVector[tn.Level](aggState.LevelValues);
                    stateUpperDesignated.LevelValues.Add(aggValue); // pass to upper
                    stateUpperDesignated.TreeNodes.Add(tn);
                    manipulator.setValue(tn.Dimmension, aggValue);
                }
            }
            else
                stateUpperDesignated.LevelValues.Add(manipulator.getValue(tnUpper.Dimmension));

            return stateUpperDesignated;
        }
    }
}
