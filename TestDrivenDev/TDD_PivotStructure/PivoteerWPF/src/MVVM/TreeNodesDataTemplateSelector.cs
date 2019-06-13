using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PivoteerWPF.MVVM
{
    class TreeNodesDataTemplateSelector : DataTemplateSelector
    {
        // those templates must be in sync with xaml
        private const string TEMPLATE_KEY_ROOT  = "RootDataTemplate"; 
        private const string TEMPLATE_KEY_FILE  = "ExcelFileDataTemplate";
        private const string TEMPLATE_KEY_SHEET = "SheetDataTemplate";

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            // TODO: switch between templates here
            if(element != null && item != null)
            {
                return element.TryFindResource(TEMPLATE_KEY_ROOT) as DataTemplate;
            }

            return null;
        }
    }
}
