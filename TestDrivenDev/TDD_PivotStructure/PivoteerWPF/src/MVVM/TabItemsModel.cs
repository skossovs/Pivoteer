using PivoteerWPF.MVVM.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PivoteerWPF.MVVM
{
    class TabItemsModel : ObservableCollection<TabItem>
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        public TabItemsModel()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<TreeViewSelectionMessage>(this, ReceiveTreeViewSelectionCommand);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<TreeViewPopulatedMessage>(this, ReceiveTreeViewPopulatedCommand);
        }
        // TODO: implement Tab addition/deletion
        private void ReceiveTreeViewPopulatedCommand(TreeViewPopulatedMessage tvPopulatedMessage)
        {
            Clear();
            foreach(var t in tvPopulatedMessage.TreeNodes)
            {
                var ti = new TabItem();
                Add(ti);
                // Provide class model for all the tabs
            }
        }


        private void ReceiveTreeViewSelectionCommand(TreeViewSelectionMessage obj)
        {
            // TODO: display certain Tab
            throw new NotImplementedException();
        }
    }
}
