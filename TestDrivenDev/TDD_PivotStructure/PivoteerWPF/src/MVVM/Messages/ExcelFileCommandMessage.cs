using GalaSoft.MvvmLight.Messaging;
using PivoteerWPF.Data;

namespace PivoteerWPF.MVVM.Messages
{
    class ExcelFileCommandMessage : MessageBase
    {
        public ExcelFileCommandMessage(int keyNode, TreeNodeCommand command, string fullPath)
        {
            KeyNode  = keyNode;
            Command  = command;
            FullPath = fullPath;
        }
        public int KeyNode { get; set; }
        public TreeNodeCommand Command { get; set; }
        public string FullPath { get; set; }
    }
}
