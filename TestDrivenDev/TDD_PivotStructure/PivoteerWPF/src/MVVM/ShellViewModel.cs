using PivoteerWPF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PivoteerWPF.Data;
using System.ComponentModel;
using PivoteerWPF.MVVM.Messages;

namespace PivoteerWPF.MVVM
{
    class ShellViewModel
    {
        private const string C_NEW = "NEW";
        private const string C_OPEN = "OPEN";
        private const string C_SAVE = "SAVE";
        private const string C_EXIT = "EXIT";

        Common.ProjectFile projectFile;

        private readonly DelegateCommand<string> _fileMenuCommand;

        public DelegateCommand<string> FileMenuCommand
        {
            get { return _fileMenuCommand; }
        }

        public ShellViewModel()
        {
            projectFile = new ProjectFile();

            _fileMenuCommand = new DelegateCommand<string>(
                (s) =>
                {
                    switch (s)
                    {
                        case C_EXIT:
                            Common.ApplicationCommands.ExitApplication();
                            break;
                        case C_NEW:
                            projectFile.CreateNew();
                            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new FileCommandMessage(C_NEW, projectFile.FullPath));
                            break;
                        case C_OPEN:
                            projectFile.Load();
                            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new FileCommandMessage(C_OPEN, projectFile.FullPath));
                            break;
                        case C_SAVE:
                            projectFile.Save();
                            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new FileCommandMessage(C_SAVE, projectFile.FullPath));
                            break;
                        default:
                            throw new Exception("Unrecognized command");
                    }
                }, //Execute
            (s) => { return true; } //CanExecute
            );
        }
    }
}
