using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivot.Accessories.Extensions
{
    internal class MatrixGetterException : Exception
    {
        private const int WindowHalfSize = 3;
        public MatrixGetterException(string[,] mtx, int x, int y) : base(convertToMessage(mtx, x, y))
        {
        }

        private static string convertToMessage(string[,] mtx, int x, int y)
        {
            var message = new StringBuilder();
            message.AppendLine("-----------------------------------");
            var xStart  = (x - WindowHalfSize) > 0 ? x - WindowHalfSize : 0;
            var yStart  = (y - WindowHalfSize) > 0 ? y - WindowHalfSize : 0;
            var xEnd    = (x + WindowHalfSize) < mtx.GetLength(0) ? x + WindowHalfSize : mtx.GetLength(0);
            var yEnd    = (y + WindowHalfSize) < mtx.GetLength(1) ? y + WindowHalfSize : mtx.GetLength(1);

            for (int yi = yStart; yi < yEnd; yi++)
            {
                for (int xi = xStart; xi < xEnd; xi++)
                {
                    message.Append((mtx[xi, yi] != null ? mtx[xi, yi] : "null") + " ");
                }
                message.AppendLine();
            }
            return message.ToString();
        }
    }
}
