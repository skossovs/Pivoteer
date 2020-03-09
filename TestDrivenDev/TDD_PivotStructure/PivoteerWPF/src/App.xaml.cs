using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PivoteerWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if(e?.Exception?.InnerException?.InnerException?.Message != null)
                logger.Error("Pivot.Accessories library exception:" + e.Exception.InnerException.InnerException.Message);
            else if(e?.Exception?.InnerException?.Message            != null)
                logger.Error("Pivoteer Control exception:"          + e.Exception.InnerException.Message);
            else if(e.Exception?.Message                             != null)
                logger.Error("Pivoteer WPF application exception:"  + e.Exception.Message);

            e.Handled = true;
        }
    }
}
