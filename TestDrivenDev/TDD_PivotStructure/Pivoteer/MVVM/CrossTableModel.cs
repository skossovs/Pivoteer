using ListToGrid;
using Pivoteer.MVVM.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivoteer.MVVM
{
    public class CrossTableModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public CrossTableModel()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CrossTablePopulateMessage>(this, ReceiveCrossTablePopulateCommand);
        }

        List<Cell> _items;
        private List<Cell> Items {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        private void ReceiveCrossTablePopulateCommand(CrossTablePopulateMessage obj)
        {
            Items = obj.TableCells;
        }
    }
}
