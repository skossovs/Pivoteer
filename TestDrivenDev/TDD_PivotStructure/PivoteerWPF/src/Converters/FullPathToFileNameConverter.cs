using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PivoteerWPF.Converters
{
    /// <summary>
    /// This converter display only the file name in tree node instead of the whole path
    /// </summary>
    class FullPathToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fullPath = value as string;
            // TODO: ugly implementation, rely on magic character, better introduce node type in parameter
            if (!string.IsNullOrEmpty(fullPath) && fullPath.Contains("\\"))
                return System.IO.Path.GetFileNameWithoutExtension(fullPath);
            else
                return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
