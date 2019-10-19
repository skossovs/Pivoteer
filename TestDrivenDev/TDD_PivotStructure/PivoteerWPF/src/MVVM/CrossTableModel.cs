using PivoteerWPF.MVVM.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM
{
    public class CrossTableModel : INotifyPropertyChanged
    {
        Dictionary<int, IEnumerable<PivotClassBase>> _pivotsDictionary;
        private int _currentItemKey;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public CrossTableModel()
        {
            _pivotsDictionary = new Dictionary<int, IEnumerable<PivotClassBase>>();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<PivotCommandMessage>(this, CrossTableProcessCommand);
        }

        private void CrossTableProcessCommand(PivotCommandMessage msg)
        {
            _currentItemKey = msg.Key;
            switch (msg.Command)
            {
                case Data.TreeNodeCommand.Run:
                    ProcessCrossTable(msg.Key, msg.Data);
                    break;
                case Data.TreeNodeCommand.Validate:
                    ValidateCrossTable(msg.Key, msg.Data);
                    break;
                default:
                    throw new Exception($"Command is not supported {msg.Command}");
            }
        }

        private void ProcessCrossTable(int key, IEnumerable<PivotClassBase> lstData)
        {
            _currentItemKey = key;
            Items = lstData;
        }
        private void ValidateCrossTable(int Key, IEnumerable<PivotClassBase> lstData)
        {
            // TODO:
        }

        public IEnumerable<PivotClassBase> Items
        {
            get
            {
                if (_pivotsDictionary.ContainsKey(_currentItemKey))
                    return _pivotsDictionary[_currentItemKey];
                else
                    return null;
            }
            set
            {
                if (_pivotsDictionary.ContainsKey(_currentItemKey))
                    _pivotsDictionary[_currentItemKey] = value;
                else
                    _pivotsDictionary.Add(_currentItemKey, value);
                OnPropertyChanged("Items");
            }
        }
    }
}
