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
    ///
    /// https://stackoverflow.com/questions/11266735/creating-custom-itemscontrol
    /// https://rachel53461.wordpress.com/2011/09/17/wpf-itemscontrol-example/
    /// </summary>
    public class PivoteerControl : ItemsControl, INotifyPropertyChanged
    {
        List<Cell> _cells;

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

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            string[,] mtx = ReloadItems();
            _cells = new List<Cell>();

            for (int i = 0; i <= mtx.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= mtx.GetUpperBound(1); j++)
                {
                    _cells.Add(new Cell() { X = i, Y = j, Value = mtx[i, j] });
                }
            }
            OnPropertyChanged("Cells");
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
            }
        }
        private string[,] ReloadItems()
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

            Type generatorType = typeof(Pivot.Accessories.PivotCoordinates.PivotGenerator<,>);
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
            object mtxResult = magicMethod.Invoke(generator, new object[] { dataAsEnum });

            return mtxResult as string[,];
        }
    }
}
