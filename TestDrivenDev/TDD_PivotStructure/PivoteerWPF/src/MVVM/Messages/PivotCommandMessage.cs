using PivoteerWPF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM.Messages
{
    class PivotCommandMessage
    {
        public PivotCommandMessage(IEnumerable<PivotClassBase> data, TreeNodeCommand command)
        {
            Data = data;
            Command = command;
        }
        public IEnumerable<PivotClassBase> Data  {  get; set;   }
        public TreeNodeCommand Command { get; set; }
    }
}
