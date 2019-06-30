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
        }
    }
}
