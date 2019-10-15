using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ListToGrid
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ListToGrid"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ListToGrid;assembly=ListToGrid"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class ListToGridControl : ItemsControl, INotifyPropertyChanged
    {
        private int _maxRows;
        private int _maxColumns;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        static ListToGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListToGridControl), new FrameworkPropertyMetadata(typeof(ListToGridControl)));
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            Cells = new ObservableCollection<Cell>();
            foreach(var c in Items)
            {
                Cells.Add(c as Cell);
            }
            // TODO: ugly
            MaxColumns = Cells.Max(c => c.X) + 1;
            MaxRows    = Cells.Max(c => c.Y) + 1;
        }

        public ObservableCollection<Cell> Cells
        {
            get;
            set;
        }

        public int MaxColumns
        {
            get { return _maxColumns; }
            set
            {
                _maxColumns = value;
                OnPropertyChanged("MaxColumns");
            }
        }
        public int MaxRows
        {
            get { return _maxRows;    }
            set
            {
                _maxRows = value;
                OnPropertyChanged("MaxRows");
            }
        }
    }
}
