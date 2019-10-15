using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomControlTestsWPF
{
    public class GridModel : ObservableCollection<ListToGrid.Cell>
    {
        public GridModel()
        {
            Add(new ListToGrid.Cell() { X = 0, Y = 0, Value = "00" });
            Add(new ListToGrid.Cell() { X = 0, Y = 1, Value = "01" });
            Add(new ListToGrid.Cell() { X = 1, Y = 0, Value = "10" });
            Add(new ListToGrid.Cell() { X = 1, Y = 1, Value = "11" });
            Add(new ListToGrid.Cell() { X = 1, Y = 2, Value = "12" });
            Add(new ListToGrid.Cell() { X = 5, Y = 6, Value = "56" });
            Add(new ListToGrid.Cell() { X = 10, Y = 10, Value = "1010" });
            Add(new ListToGrid.Cell() { X = 11, Y = 11, Value = "1010" });
            Add(new ListToGrid.Cell() { X = 12, Y = 12, Value = "1010" });
            Add(new ListToGrid.Cell() { X = 13, Y = 13, Value = "1010" });
            Add(new ListToGrid.Cell() { X = 14, Y = 14, Value = "1010" });
            Add(new ListToGrid.Cell() { X = 15, Y = 15, Value = "1010" });
            Add(new ListToGrid.Cell() { X = 16, Y = 16, Value = "1010" });
            Add(new ListToGrid.Cell() { X = 17, Y = 17, Value = "1010" });
            Add(new ListToGrid.Cell() { X = 18, Y = 18, Value = "1010" });
            Add(new ListToGrid.Cell() { X = 50, Y = 50, Value = "5050" });
        }
    }
}
