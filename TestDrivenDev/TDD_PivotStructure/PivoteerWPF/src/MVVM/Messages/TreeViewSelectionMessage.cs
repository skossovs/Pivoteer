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
        public TreeViewSelectionMessage(int key, string leaf, TreeNodeType t, string path)
        {
            Key = key;
            Leaf = leaf;
            Type = t;
            Path = path;
        }
        public int Key { get; set; }
        public string Leaf { get; set; }
        public string Path { get; set; }
        public TreeNodeType Type { get; set; }
    }
}
