using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM
{
    class FileCommandMessage : MessageBase
    {
        public FileCommandMessage(string command, string path)
        {
            Command = command;
            Path = path;
        }
        public string Command { get; set; }
        public string Path { get; set; }
    }
}
