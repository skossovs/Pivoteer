using PivoteerWPF.Common;
using PivoteerWPF.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM
{
    class FileTreeViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private Project     _content;
        private List<Group> _treeViewContentRepresentation;
        public FileTreeViewModel()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<FileCommandMessage>(this, ReceiveFileCommand);
        }

        private void ReceiveFileCommand(FileCommandMessage fileCommand)
        {
            switch(fileCommand.Command)
            {
                case "NEW":
                    _content = new Project();
                    ApplicationCommands.SaveJsonObject(fileCommand.Path, _content);
                    TreeData = ConvertFromJsonToGroups();
                    break;
                case "OPEN":
                    _content = (Project) ApplicationCommands.ReadJsonObject(fileCommand.Path, typeof(Project)); // TODO: by default there is always a project. not right
                    if(_content != null)
                        TreeData = ConvertFromJsonToGroups();
                    break;
                case "SAVE":
                    ApplicationCommands.SaveJsonObject(fileCommand.Path, _content);
                    break;
                default:
                    throw new Exception($"Unknnown File Menu command {fileCommand.Command}");
            }
        }

        public List<Group> ConvertFromJsonToGroups()
        {
            // Name
            var RootGroup = new Group() {
                Key = 0,
                Name = _content?.ProjectDescription,
                SubGroups = new List<Group>(),
                Entries = new List<Entry>()
            };

            var RootGroups = new List<Group>() { RootGroup };

            _content?.ExcelFiles?.ForEach(f =>
            {
                var g = f.ExcelFileDataToGroup();
                RootGroup.SubGroups.Add(g);

                f?.Sheets?.ForEach(s =>
                {
                    g.Entries.Add(new Entry() { Name = s.SheetName });
                });
            });

            return RootGroups;
        }

        public List<Group> TreeData {
            get
            {
                return _treeViewContentRepresentation;
            }
            set
            {
                _treeViewContentRepresentation = value;
                OnPropertyChanged("TreeData");
            }
        }
    }
}
