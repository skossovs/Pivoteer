using PivoteerWPF.Common;
using PivoteerWPF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM
{
    class FileTreeViewModel
    {
        private Project _content;
        public FileTreeViewModel()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<FileCommandMessage>(this, ReceiveFileCommand);
        }

        private void ReceiveFileCommand(FileCommandMessage fileCommand)
        {
            switch(fileCommand.Command)
            {
                case "NEW":
                    _content = new Project();
                    ApplicationCommands.SaveJsonObject(fileCommand.Path, _content);
                    break;
                case "OPEN":
                    _content = (Project) ApplicationCommands.ReadJsonObject(fileCommand.Path, typeof(Project));
                    break;
                case "SAVE":
                    ApplicationCommands.SaveJsonObject(fileCommand.Path, _content);
                    break;
                default:
                    throw new Exception($"Unknnown File Menu command {fileCommand.Command}");
            }
        }
    }
}
