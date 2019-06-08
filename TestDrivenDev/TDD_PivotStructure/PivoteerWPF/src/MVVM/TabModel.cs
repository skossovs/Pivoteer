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
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<TreeViewPopulatedMessage>(this, ReceiveTreeViewPopulatedCommand);
        }
        // TODO: implement Tab addition/deletion
        private void ReceiveTreeViewPopulatedCommand(TreeViewPopulatedMessage obj)
        {
            // TODO: display certain Tab
            throw new NotImplementedException();
        }


        private void ReceiveTreeViewSelectionCommand(TreeViewSelectionMessage obj)
        {
            // TODO: display certain Tab
            throw new NotImplementedException();
        }
    }
}
