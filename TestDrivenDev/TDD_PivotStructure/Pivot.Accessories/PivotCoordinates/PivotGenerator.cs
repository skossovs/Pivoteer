﻿using Pivot.Accessories.Mapping;
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
        public string[,] GeneratePivot(IEnumerable<T> data)
        {
            #region STAGE I: pivot matrix with no aggregations
            var dicX = _dictionaryGenerator.GenerateXDictionary(data);
            var dicY = _dictionaryGenerator.GenerateYDictionary(data);

            string[,] matrix = new string[dicX.Count + _typeWrapper.XType.MaxDim, dicY.Count + _typeWrapper.YType.MaxDim];

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
            // TODO: clean up
            //            mmx.AggregationFunctionVector = utilsAggregation.GenerateSummariesFunctionArray(_typeWrapper.XType.MaxDim);
            //            mmy.AggregationFunctionVector = utilsAggregation.GenerateSummariesFunctionArray(_typeWrapper.YType.MaxDim);

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

            return matrix;
        }
    }
}