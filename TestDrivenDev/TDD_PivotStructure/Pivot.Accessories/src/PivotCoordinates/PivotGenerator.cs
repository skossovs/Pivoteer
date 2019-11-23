using Pivot.Accessories.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.PivotCoordinates
{
    public class PivotGenerator<T, TAggregator> 
        where T : class
        where TAggregator : class
    {
        DictionaryGenerator<T, TAggregator> _dictionaryGenerator;
        TypeWrapper<T, TAggregator> _typeWrapper;
        public PivotGenerator(TypeWrapper<T, TAggregator> t)
        {
            _typeWrapper = t;
            _dictionaryGenerator = new DictionaryGenerator<T, TAggregator>(t);
        }


        private List<HeaderNode> GenerateRowHeaders(SortedDictionary<FieldList, int> dicY, int depth)
        {
            List<HeaderNode> headerNodes = new List<HeaderNode>();

            var fieldsBuffer = dicY.OrderBy(kv => kv.Value).First().Key.Select(s => new HeaderNode() { Index = -1, Length = -1, Level = -1, Text = string.Empty }).ToList();

            foreach (var fieldsIndex in dicY.OrderBy(kv => kv.Value))
            {
                var fieldList = fieldsIndex.Key;

                for (int i = 0; i < depth; i++)
                {
                    var newHeaderNode = new HeaderNode()
                    {
                        Index = fieldsIndex.Value,
                        Level = depth - i, // inverse the level
                        Text = fieldList[i],
                        Length = 1,
                    };

                    if (fieldsBuffer[i].Text != newHeaderNode.Text || newHeaderNode.Text.Length == 0)
                    {
                        fieldsBuffer[i] = newHeaderNode;
                        headerNodes.Add(newHeaderNode);
                    }
                    else
                    {
                        fieldsBuffer[i].Length++;
                    }
                }

            }

            return headerNodes;
        }


        private List<HeaderNode> GenerateColumnHeaders(SortedDictionary<FieldList, int> dicX, int depth)
        {
            List<HeaderNode> headerNodes = new List<HeaderNode>();

            var fieldsBuffer = dicX.OrderBy(kv => kv.Value).First().Key.Select( s => new HeaderNode() { Index = -1, Length = -1, Level = -1, Text = string.Empty  }).ToList();

            foreach (var fieldsIndex in dicX.OrderBy(kv => kv.Value))
            {
                var fieldList = fieldsIndex.Key;

                for (int i=0; i < depth; i++)
                {
                    var newHeaderNode = new HeaderNode()
                    {
                        Index = fieldsIndex.Value,
                        Level = depth - i, // inverse the level
                        Text = fieldList[i],
                        Length = 1,
                    };

                    if (fieldsBuffer[i].Text != newHeaderNode.Text || newHeaderNode.Text.Length == 0)
                    {
                        fieldsBuffer[i] = newHeaderNode;
                        headerNodes.Add(newHeaderNode);
                    }
                    else
                    {
                        fieldsBuffer[i].Length++;
                    }
                }

            }

            return headerNodes;
        }


        public GeneratedData GeneratePivot(IEnumerable<T> data)
        {
            #region STAGE I: pivot matrix with no aggregations
            var dicX = _dictionaryGenerator.GenerateXDictionary(data);
            var dicY = _dictionaryGenerator.GenerateYDictionary(data);

            // TODO: build up column and row headers trees
            var columnHeaders = GenerateColumnHeaders(dicX, _typeWrapper.XType.MaxDim);
            var rowHeaders = GenerateRowHeaders(dicY, _typeWrapper.YType.MaxDim);

            string[,] matrix = new string[dicX.Count + _typeWrapper.XType.MaxDim, dicY.Count + _typeWrapper.YType.MaxDim];

            Func<T, int, string> getterFuncX  = (obj, j) => _typeWrapper.XType.GetField(obj, j);
            Func<T, int, string> getterFuncY  = (obj, j) => _typeWrapper.YType.GetField(obj, j);
            Func<T, decimal?> getterFuncValue = (obj)    => _typeWrapper.VType.GetValue(obj);

            var utilsAggregation = new AggregationTreeGenerator<T, TAggregator>(_typeWrapper);

            // TODO: inner, outer matrices can be populated in parallel
            // populate decimal values (inner matrix)
            foreach (var element in data)
            {
                decimal? value       = getterFuncValue(element);
                T        unclosure_t = element;

                // convert physical fields to ListFields X and Y
                var fldListX = new FieldList();
                for (int i = 0; i < _typeWrapper.XType.MaxDim; i++)
                {
                    int unclosure_i = i;
                    fldListX.Add(getterFuncX(unclosure_t, unclosure_i));
                }

                var fldListY = new FieldList();
                for (int i = 0; i < _typeWrapper.YType.MaxDim; i++)
                {
                    int unclosure_i = i;
                    fldListY.Add(getterFuncY(unclosure_t, unclosure_i));
                }

                // Find coordinates X and Y from dictionaries
                int X = dicX[fldListX];
                int Y = dicY[fldListY];

                // put value in matrix
                matrix[X, Y] = Convert.ToString(value);

            }

            // populate outer field names
            foreach (var fldListX in dicX)
            {
                for (int i = 0; i < _typeWrapper.XType.MaxDim; i++)
                {
                    matrix[fldListX.Value, i] = fldListX.Key.ElementAt(_typeWrapper.XType.MaxDim - i - 1); // reverse field list order
                }
            }
            foreach (var fldListY in dicY)
            {
                for (int i = 0; i < _typeWrapper.YType.MaxDim; i++)
                {
                    matrix[i, fldListY.Value] = fldListY.Key.ElementAt(_typeWrapper.YType.MaxDim - i - 1); // reverse field list order
                }
            }
            #endregion

            #region STAGE II: Prepare aggregation tree
            var mmx = new MatrixManipulator();
            var mmy = new MatrixManipulator();

            mmx.AggregationFunctionVector = _typeWrapper.XType.AggregationFunctionsByLevel;
            mmy.AggregationFunctionVector = _typeWrapper.YType.AggregationFunctionsByLevel;

            var aggXSeedTree = utilsAggregation.GenerateXAggregationTree(dicX);
            var aggYSeedTree = utilsAggregation.GenerateYAggregationTree(dicY);

            var daX = new DimmensionAggregator(aggXSeedTree);
            var daY = new DimmensionAggregator(aggYSeedTree);
            #endregion

            #region STAGE III: Traverse matrix
            // Traverse by X
            for (int x = _typeWrapper.XType.MaxDim; x < dicX.Count + _typeWrapper.XType.MaxDim; x++)
            {
                mmy.getValue = utilsAggregation.CreateYGetter(x, matrix);
                mmy.setValue = utilsAggregation.CreateYSetter(x, matrix);
                daY.DrillDownTree(mmy);
            }

            // Traverse by Y
            for (int y = _typeWrapper.YType.MaxDim; y < dicY.Count + _typeWrapper.YType.MaxDim; y++)
            {
                mmx.getValue = utilsAggregation.CreateXGetter(y, matrix);
                mmx.setValue = utilsAggregation.CreateXSetter(y, matrix);
                daX.DrillDownTree(mmx);
            }
            #endregion

            var result                    = new GeneratedData();
            result.Matrix                 = matrix;
            result.Row_Hierarchy_Depth    = _typeWrapper.XType.MaxDim;
            result.Column_Hierarchy_Depth = _typeWrapper.YType.MaxDim;
            result.ColumnHeaders       = columnHeaders.ToList();
            result.RowHeaders          = rowHeaders.ToList();

            return result;
        }
    }
}
