using PivoteerWPF.Data;
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
                var tn = item as TreeNode;
                switch(tn.Type)
                {
                    case TreeNodeType.Root:
                        return element.TryFindResource(TEMPLATE_KEY_ROOT) as DataTemplate;
                    case TreeNodeType.ExcelFile:
                        return element.TryFindResource(TEMPLATE_KEY_FILE) as DataTemplate;
                    case TreeNodeType.ExcelSheet:
                        return element.TryFindResource(TEMPLATE_KEY_SHEET) as DataTemplate;
                    case TreeNodeType.Invalid:
                        throw new NotImplementedException("TODO: Invalid case must be implemented!!");
                }
            }

            return null;
        }
    }
}
