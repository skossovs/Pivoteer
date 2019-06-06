using PivoteerWPF.MVVM.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM
{
    class TabModel
    {
        public TabModel()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<TreeViewSelectionMessage>(this, ReceiveTreeViewSelectionCommand);
        }

        private void ReceiveTreeViewSelectionCommand(TreeViewSelectionMessage obj)
        {
            // TODO: display certain Tab
            throw new NotImplementedException();
        }
    }
}
