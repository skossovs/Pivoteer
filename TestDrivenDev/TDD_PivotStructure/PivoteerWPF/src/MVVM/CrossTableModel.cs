using PivoteerWPF.MVVM.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM
{
    public class CrossTableModel : ObservableCollection<PivotClassBase>
    {
        public CrossTableModel()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<PivotCommandMessage>(this, CrossTableProcessCommand);
        }

        private void CrossTableProcessCommand(PivotCommandMessage msg)
        {
            switch(msg.Command)
            {
                case Data.TreeNodeCommand.Run:
                    ProcessCrossTable(msg.Data);
                    break;
                case Data.TreeNodeCommand.Validate:
                    ValidateCrossTable(msg.Data);
                    break;
                default:
                    throw new Exception($"Command is not supported {msg.Command}");
            }
        }

        private void ProcessCrossTable(IEnumerable<PivotClassBase> lstData)
        {
            // TODO:
        }
        private void ValidateCrossTable(IEnumerable<PivotClassBase> lstData)
        {
            // TODO:
        }

    }
}
