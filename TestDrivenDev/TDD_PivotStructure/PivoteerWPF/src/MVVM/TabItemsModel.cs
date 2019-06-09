using PivoteerWPF.MVVM.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PivoteerWPF.MVVM
{
    class TabItemsModel : INotifyPropertyChanged
    {
        List<TabItem> _tabItems;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        public TabItemsModel()
        {
            _tabItems = new List<TabItem>();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<TreeViewSelectionMessage>(this, ReceiveTreeViewSelectionCommand);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<TreeViewPopulatedMessage>(this, ReceiveTreeViewPopulatedCommand);
        }
        // TODO: implement Tab addition/deletion
        private void ReceiveTreeViewPopulatedCommand(TreeViewPopulatedMessage tvPopulatedMessage)
        {
            _tabItems.Clear();
            foreach(var t in tvPopulatedMessage.TreeNodes)
            {
                _tabItems.Add(new TabItem());
                // Provide class model for all the tabs
            }
            OnPropertyChanged("TabItems");

        }


        private void ReceiveTreeViewSelectionCommand(TreeViewSelectionMessage obj)
        {
            // TODO: display certain Tab
            throw new NotImplementedException();
        }

        public IList<TabItem> TabItems
        {
            get
            {
                return _tabItems;
            }
        }
    }
}
