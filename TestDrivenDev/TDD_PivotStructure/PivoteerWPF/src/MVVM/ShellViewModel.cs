using PivoteerWPF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM
{
    class ShellViewModel
    {
        private const string C_NEW  = "NEW";
        private const string C_OPEN = "OPEN";
        private const string C_SAVE = "SAVE";
        private const string C_EXIT = "EXIT";

        Common.ProjectFile<Project> projectFile;

        private readonly DelegateCommand<string> _fileMenuCommand;

        public DelegateCommand<string> FileMenuCommand
        {
            get { return _fileMenuCommand; }
        }

        public ShellViewModel()
        {
            projectFile = new ProjectFile<Project>();

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
                            break;
                        case C_OPEN:
                        case C_SAVE:
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
