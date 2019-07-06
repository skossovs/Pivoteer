using Pivoteer.MVVM.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivoteer.MVVM
{
    public class ColumnHeadersModel
    {
        public ColumnHeadersModel()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<HeadersPopulateMessage>(this, ReceiveColumnHeadersPopulateCommand);
        }

        private void ReceiveColumnHeadersPopulateCommand(HeadersPopulateMessage obj)
        {
            throw new NotImplementedException();
        }
    }
}
