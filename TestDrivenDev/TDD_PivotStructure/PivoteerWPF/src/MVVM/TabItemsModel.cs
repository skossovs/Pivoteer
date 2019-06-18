using PivoteerWPF.Data;
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
    class TabItemsModel : INotifyPropertyChanged
    {
        IList<TreeNode> _treeNodes;
        TreeNode        _treeNode;
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
            TreeNodes = tvPopulatedMessage.TreeNodes;
        }


        private void ReceiveTreeViewSelectionCommand(TreeViewSelectionMessage tvSelectedMessage)
        {
            SelectedNode = TreeNodes.First(t => t.Key == tvSelectedMessage.Key);
        }

        public IList<TreeNode> TreeNodes
        {
            get
            {
                return _treeNodes;
            }
            set
            {
                _treeNodes = value;
                OnPropertyChanged("TreeNodes");
            }
        }

        public TreeNode SelectedNode
        {
            get
            {
                return _treeNode;
            }
            set
            {
                _treeNode = value;
                OnPropertyChanged("SelectedNode");
            }
        }

    }
}
