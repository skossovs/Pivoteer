using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM
{
    class FileTreeViewModel
    {

        public FileTreeViewModel()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<FileCommandMessage>(this, ReceiveFileCommand);
        }

        private void ReceiveFileCommand(FileCommandMessage obj)
        {
            throw new NotImplementedException();
        }
    }
}
