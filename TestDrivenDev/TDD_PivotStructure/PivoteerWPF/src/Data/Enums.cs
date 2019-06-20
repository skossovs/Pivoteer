using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.Data
{
    public enum TreeNodeType { Invalid = -1, Root=0, ExcelFile=1, ExcelSheet=2 };
    public enum TreeNodeCommand { Add, Delete, Update, Validate };
}
