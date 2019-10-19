using Pivoteer.MVVM.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivoteer.MVVM
{
    public class RowHeadersModel
    {
        public RowHeadersModel()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<HeadersPopulateMessage>(this, ReceiveRowHeadersPopulateCommand);
        }

        private void ReceiveRowHeadersPopulateCommand(HeadersPopulateMessage obj)
        {
            throw new NotImplementedException();
        }
    }
}
