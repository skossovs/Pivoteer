using GalaSoft.MvvmLight.Messaging;
using PivoteerWPF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM.Messages
{
    class TreeViewSelectionMessage : MessageBase
    {
        public int Key { get; set; }
        public TreeViewSelectionMessage(int key)
        {
            Key = key;
        }
    }
}
