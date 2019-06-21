using GalaSoft.MvvmLight.Messaging;
using PivoteerWPF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.src.MVVM.Messages
{
    class ProjectLoadedMessage : MessageBase
    {
        List<TreeNode> NodeList { get; set; }
        public ProjectLoadedMessage(List<TreeNode> projectOnTreeView)
        {
            NodeList = projectOnTreeView;
        }
    }
}
