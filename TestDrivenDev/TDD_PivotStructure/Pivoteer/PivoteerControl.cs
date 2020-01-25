using ListToGrid;
using Pivot.Accessories.PivotCoordinates;
using Pivoteer.MVVM.Messages;
using System;
using System.Collections.Generic;
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

namespace Pivoteer
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pivoteer"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pivoteer;assembly=Pivoteer"
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
    /// Titans Shoulders:
    /// https://stackoverflow.com/questions/11266735/creating-custom-itemscontrol
    /// https://rachel53461.wordpress.com/2011/09/17/wpf-itemscontrol-example/
    /// https://stackoverflow.com/questions/38842082/create-a-custom-control-with-the-combination-of-multiple-controls-in-wpf-c-sharp
    /// </summary>
    public class PivoteerControl : ItemsControl, INotifyPropertyChanged
    {
        List<Cell> _cells;

        public MVVM.CrossTableModel    CrossTableModel   { get; private set; }
        public MVVM.RowHeadersModel    RowHeaderModel    { get; private set; }
        public MVVM.ColumnHeadersModel ColumnHeaderModel { get; private set; }

        private ListToGridControl _crossTableGrid;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        static PivoteerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PivoteerControl), new FrameworkPropertyMetadata(typeof(PivoteerControl)));
        }

        public PivoteerControl()
        {
            CrossTableModel   = new MVVM.CrossTableModel();
            RowHeaderModel    = new MVVM.RowHeadersModel();
            ColumnHeaderModel = new MVVM.ColumnHeadersModel();
        }

        public override void OnApplyTemplate()
        {
            _crossTableGrid    = this.GetTemplateChild("CrossTable") as ListToGridControl;

            Binding b = new Binding("Cells"); // Cells as source
            b.Source = this; // outer object as Source
            b.Mode = BindingMode.OneWay;
            // _crossTableGrid                        - inner object as target
            // ListToGridControl.ItemsSourceProperty  - inner object itemsSource
            BindingOperations.SetBinding(_crossTableGrid, ListToGridControl.ItemsSourceProperty, b);

            base.OnApplyTemplate();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            var data = ReloadItems();
            string[,] mtx = data.Matrix;
            _cells = new List<Cell>();

            // Populate the Columns
            data.ColumnHeaders.ForEach(h =>
            {
                _cells.Add(new Cell() { X=h.Index, Y=h.Level, XSpan=h.Length, YSpan=1, Value=h.Text});
            });
            // Populate the Rows
            data.RowHeaders.ForEach(h =>
            {
                _cells.Add(new Cell() { Y = h.Index, X = h.Level, YSpan = h.Length, XSpan = 1, Value = h.Text });
            });

            // Populate the values
            for (int i = data.Row_Hierarchy_Depth; i <= mtx.GetUpperBound(0); i++)
            {
                for (int j = data.Column_Hierarchy_Depth; j <= mtx.GetUpperBound(1); j++)
                {
                    _cells.Add(new Cell() { X = i, Y = j, Value = mtx[i, j], YSpan=1, XSpan=1 });
                }
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new CrossTablePopulateMessage(_cells));
            Cells = _cells;
        }

        public List<Cell> Cells
        {
            get
            {
                return _cells;
            }
            set
            {
                _cells = value;
                OnPropertyChanged("Cells");
            }
        }
        private GeneratedData ReloadItems()
        {
            // TODO: not beautiful
            // get the item's type
            Object o = this.Items.CurrentItem;
            Type TObjectType = o.GetType();
            // get Aggregation Functions class
            var customAttributes = TObjectType.CustomAttributes;
            var aggregationType = customAttributes.FirstOrDefault(t => t.AttributeType.Name == "Aggregators")
                .NamedArguments.FirstOrDefault().TypedValue;
            // create classes dynamically
            Type   typeWrapperType = typeof(Pivot.Accessories.Mapping.TypeWrapper<,>);
            Type[] typeArgs = { TObjectType, (aggregationType.Value as Type) };
            var makeTypeWrapper = typeWrapperType.MakeGenericType(typeArgs);
            object typeWrapper = Activator.CreateInstance(makeTypeWrapper);

            Type generatorType = typeof(PivotGenerator<,>);
            Type[] typeArgs1 = { TObjectType, (aggregationType.Value as Type) };
            var makeGenerator = generatorType.MakeGenericType(typeArgs1);
            object generator = Activator.CreateInstance(makeGenerator, typeWrapper);

            // Items.Cast<T>()
            var method = typeof(Enumerable).GetMethod("Cast");
            var generic = method.MakeGenericMethod(TObjectType);
            object dataAsEnum = generic.Invoke(null, new object[] { Items });

            // IEnumerable<T>
            Type enumType = typeof(IEnumerable<>);
            Type[] enumArgType = { TObjectType };
            var genericEnumClass = enumType.MakeGenericType(enumArgType);

            //var mtx = generator.GeneratePivot(data);
            var magicMethod = makeGenerator.GetMethod("GeneratePivot", new[] { genericEnumClass });
            var mtxResult = magicMethod.Invoke(generator, new object[] { dataAsEnum }) as GeneratedData;

            return mtxResult;
        }
    }
}
