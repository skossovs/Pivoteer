using PivoteerWPF.MVVM.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace PivoteerWPF.MVVM.Behaviors
{
    public class BindableSelectedItemBehavior : Behavior<TreeView>
    {
        #region SelectedItem Property

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(BindableSelectedItemBehavior), new UIPropertyMetadata(null, OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var item = e.NewValue as TreeViewItem;
            if (item != null)
            {
                item.SetValue(TreeViewItem.IsSelectedProperty, true);
            }
        }

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            }
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.SelectedItem = e.NewValue;

            var treeNodeType = Data.TreeNodeType.Root;
            string name = string.Empty;
            string path = string.Empty;
            string delimeter = string.Empty + (char)0x00;
            int key = -1;

            if (e.NewValue.GetType() == typeof(Group))
            {
                var group = e.NewValue as Group;
                if(group.Path.Contains(delimeter))  // root node has no delimeter
                    treeNodeType = Data.TreeNodeType.ExcelFile;
                name = group.Name;
                path = group.Path;
                key = group.Key;
            }
            else if (e.NewValue.GetType() == typeof(Entry))
            {
                treeNodeType = Data.TreeNodeType.ExcelSheet;
                var entry = e.NewValue as Entry;
                name = entry.Name;
                path = entry.Path;
                key = entry.Key;
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new TreeViewSelectionMessage(key, name, treeNodeType, path));
        }
    }
}
