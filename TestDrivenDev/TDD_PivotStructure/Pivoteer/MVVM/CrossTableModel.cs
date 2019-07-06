﻿using Pivoteer.MVVM.Messages;
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

        public List<Cell> Items { get; private set; }

        private void ReceiveCrossTablePopulateCommand(CrossTablePopulateMessage obj)
        {
            Items = obj.TableCells;
            OnPropertyChanged("Items");
        }
    }
}
