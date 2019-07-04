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
        }
    }
}
