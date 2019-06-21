using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM.Messages
{
    class ExcelFileCommandMessage : MessageBase
    {
        public ExcelFileCommandMessage(int keyNode, string command, string fullPath)
        {
            KeyNode  = keyNode;
            Command  = command;
            FullPath = fullPath;
        }
        public int KeyNode { get; set; }
        public string Command { get; set; }
        public string FullPath { get; set; }

    }
}
