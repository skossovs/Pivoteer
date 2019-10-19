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
        public PivotCommandMessage(int key, IEnumerable<PivotClassBase> data, TreeNodeCommand command)
        {
            Data = data;
            Command = command;
        }
        public int Key { get; set; }
        public IEnumerable<PivotClassBase> Data  {  get; set;   }
        public TreeNodeCommand Command { get; set; }
    }
}
