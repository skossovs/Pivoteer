﻿using PivoteerWPF.Common;
using PivoteerWPF.Data;
using PivoteerWPF.MVVM.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM
{
    // TODO: this macros class is for temporary use only. Must be dropped after pivot load become stable
    class FixedMacrosModel : INotifyPropertyChanged
    {
        private const string C_STOCK_PRICES  = "StockPrices";
        private const string C_STOCK_MKTCAP  = "StockMktCap";
        private const string C_OPEN          = "OPEN";

        private const string C_JSON_WITH_EXCEL  = @"..\Debug\ProjectSamples\Excel.json";
        private const int    C_KEY_STOCK_PRICES = 67616754;
        private const int    C_KEY_STOCK_MKTCAP = 1544075414;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public FixedMacrosModel()
        {
            _macrosMenuCommand = new DelegateCommand<string>(
                (s) =>
                {
                    switch (s)
                    {
                        case C_STOCK_PRICES:
                            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new FileCommandMessage(C_OPEN, C_JSON_WITH_EXCEL));
                            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new TreeViewSelectionMessage(C_KEY_STOCK_PRICES));
                            break;
                        case C_STOCK_MKTCAP:
                            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new FileCommandMessage(C_OPEN, C_JSON_WITH_EXCEL));
                            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new TreeViewSelectionMessage(C_KEY_STOCK_MKTCAP));
                            break;
                        default:
                            throw new Exception("Unrecognized command");
                    }
                }, //Execute
            (s) => { return true; } //CanExecute
            );
        }

        private readonly DelegateCommand<string> _macrosMenuCommand;


        public DelegateCommand<string> MacrosMenuCommand
        {
            get { return _macrosMenuCommand; }
        }
    }
}
