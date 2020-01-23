using NLog;
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
        static Logger logger;

        static PivotGenerator()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        public PivotGenerator(TypeWrapper<T, TAggregator> t)
        {
            _typeWrapper = t;
            _dictionaryGenerator = new DictionaryGenerator<T, TAggregator>(t);
        }

        private List<HeaderNode> GenerateRowHeaders(SortedDictionary<FieldList, int> dicY, int depth)
        {
            List<HeaderNode> headerNodes = new List<HeaderNode>();

            logger.Info($"Received the following row metrics: Row Depth = {depth}, RowHeaders Count = {dicY.Count}");

            var fieldsBuffer = dicY.OrderBy(kv => kv.Value).First().Key.Select(s => new HeaderNode() { Index = -1, Length = -1, Level = -1, Text = string.Empty }).ToList();

            foreach (var fieldsIndex in dicY.OrderBy(kv => kv.Value))
            {
                var fieldList = fieldsIndex.Key;

                for (int i = 0; i < depth; i++)
                {
                    var newHeaderNode = new HeaderNode()
                    {
                        Index = fieldsIndex.Value,
                        Level = depth - i - 1, // inverse the level TODO: make it generic
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

            logger.Info($"Received the following column metrics: Column Depth = {depth}, ColumnHeaders Count = {dicX.Count}");

            var fieldsBuffer = dicX.OrderBy(kv => kv.Value).First().Key.Select( s => new HeaderNode() { Index = -1, Length = -1, Level = -1, Text = string.Empty  }).ToList();

            foreach (var fieldsIndex in dicX.OrderBy(kv => kv.Value))
            {
                var fieldList = fieldsIndex.Key;

                for (int i=0; i < depth; i++)
                {
                    var newHeaderNode = new HeaderNode()
                    {
                        Index = fieldsIndex.Value,
                        Level = depth - i - 1, // inverse the level TODO: make it generic
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

            logger.Info("*** Generate new pivot ***");
            // build up column and row headers trees
            var columnHeaders = Task.Run(() => GenerateColumnHeaders(dicX, _typeWrapper.XType.MaxDim)).Result;
            var rowHeaders    = Task.Run(() => GenerateRowHeaders   (dicY, _typeWrapper.YType.MaxDim)).Result;

            string[,] matrix  = new string[dicX.Count + _typeWrapper.YType.MaxDim, dicY.Count + _typeWrapper.XType.MaxDim];

            //Task.Run(() => );
            Func<T, int, string> getterFuncX  = (obj, j) => _typeWrapper.XType.GetField(obj, j);
            Func<T, int, string> getterFuncY  = (obj, j) => _typeWrapper.YType.GetField(obj, j);
            Func<T, decimal?> getterFuncValue = (obj)    => _typeWrapper.VType.GetValue(obj);

            var utilsAggregation = new AggregationTreeGenerator<T, TAggregator>(_typeWrapper);

            // populate decimal values (inner matrix)
            foreach (var element in data)
            {
                decimal? value       = getterFuncValue(element);
                T        unclosure_t = element;

                // convert physical fields to ListFields X and Y
                var fldListX = new FieldList();
                var fldListY = new FieldList();

                var taskFieldListPopulationX = Task.Run(() =>
                {
                    for (int i = 0; i < _typeWrapper.XType.MaxDim; i++)
                    {
                        int unclosure_i = i;
                        fldListX.Add(getterFuncX(unclosure_t, unclosure_i));
                    }
                });

                var taskFieldListPopulationY = Task.Run(() =>
                {
                    for (int i = 0; i < _typeWrapper.YType.MaxDim; i++)
                    {
                        int unclosure_i = i;
                        fldListY.Add(getterFuncY(unclosure_t, unclosure_i));
                    }
                });

                Task.WaitAll(taskFieldListPopulationX, taskFieldListPopulationY);

                // Find coordinates X and Y from dictionaries
                int X = dicX[fldListX];
                int Y = dicY[fldListY];

                // put value in matrix
                matrix[X, Y] = Convert.ToString(value);
            }

            // populate outer field names
            var taskMatrixPopulateByX =  Task.Run(() =>
            {
                foreach (var fldListX in dicX)
                {
                    for (int i = 0; i < _typeWrapper.XType.MaxDim; i++)
                    {
                        matrix[fldListX.Value, i] = fldListX.Key.ElementAt(_typeWrapper.XType.MaxDim - i - 1); // reverse field list order
                    }
                }
            });

            var taskMatrixPopulateByY = Task.Run(() =>
            {
                foreach (var fldListY in dicY)
                {
                    for (int i = 0; i < _typeWrapper.YType.MaxDim; i++)
                    {
                        matrix[i, fldListY.Value] = fldListY.Key.ElementAt(_typeWrapper.YType.MaxDim - i - 1); // reverse field list order
                    }
                }
            });

            Task.WaitAll(taskMatrixPopulateByX, taskMatrixPopulateByY);
            #endregion

            #region STAGE II: Prepare aggregation tree
            var mmx = new MatrixManipulator();
            var mmy = new MatrixManipulator();

            mmx.AggregationFunctionVector = _typeWrapper.XType.AggregationFunctionsByLevel;
            mmy.AggregationFunctionVector = _typeWrapper.YType.AggregationFunctionsByLevel;

            var aggXSeedTree = Task.Run(() => utilsAggregation.GenerateXAggregationTree(dicX)).Result;
            var aggYSeedTree = Task.Run(() => utilsAggregation.GenerateYAggregationTree(dicY)).Result;

            var daX = Task.Run(() => new DimmensionAggregator(aggXSeedTree)).Result;
            var daY = Task.Run(() => new DimmensionAggregator(aggYSeedTree)).Result;
            #endregion

            #region STAGE III: Traverse matrix and calculate aggregations
            // For each calcullable column or X create getters/setters functions calculating by-Y-summary values
            var taskCreateGetterSetterByX = Task.Run(() =>
            {
                for (int x = _typeWrapper.XType.MaxDim - 1; x < (dicX.Count - 1) + _typeWrapper.XType.MaxDim; x++)
                {
                    mmy.getValue = utilsAggregation.CreateYGetter(x, matrix);
                    mmy.setValue = utilsAggregation.CreateYSetter(x, matrix);
                    daY.DrillDownTree(mmy);
                }
            });

            // For each calcullable row or Y create getters/setters functions calculating by-X-summary values
            var taskCreateGetterSetterByY = Task.Run(() =>
            {
                for (int y = _typeWrapper.YType.MaxDim - 1; y < (dicY.Count - 1) + _typeWrapper.YType.MaxDim; y++)
                {
                    mmx.getValue = utilsAggregation.CreateXGetter(y, matrix);
                    mmx.setValue = utilsAggregation.CreateXSetter(y, matrix);
                    daX.DrillDownTree(mmx);
                }
            });

            Task.WaitAll(taskCreateGetterSetterByX, taskCreateGetterSetterByY);
            #endregion

            var result                    = new GeneratedData();
            result.Matrix                 = matrix;
            result.Row_Hierarchy_Depth    = _typeWrapper.YType.MaxDim;
            result.Column_Hierarchy_Depth = _typeWrapper.XType.MaxDim;
            result.ColumnHeaders          = columnHeaders.ToList();
            result.RowHeaders             = rowHeaders.ToList();

            return result;
        }
    }
}
