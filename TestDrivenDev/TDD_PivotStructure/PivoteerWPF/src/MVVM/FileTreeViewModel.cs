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

            _content?.ExcelFiles.ForEach(f =>
            {
                RootGroup.SubGroups.Add(f.ExcelFileDataToGroup());
            });


            //var Groups = new List<Group>();
            //Group grp1 = new Group() { Key = 1, Name = "Group 1", SubGroups = new List<Group>(), Entries = new List<Entry>() };
            //Group grp2 = new Group() { Key = 2, Name = "Group 2", SubGroups = new List<Group>(), Entries = new List<Entry>() };
            //Group grp3 = new Group() { Key = 3, Name = "Group 3", SubGroups = new List<Group>(), Entries = new List<Entry>() };
            //Group grp4 = new Group() { Key = 4, Name = "Group 4", SubGroups = new List<Group>(), Entries = new List<Entry>() };

            ////grp1
            //grp1.Entries.Add(new Entry() { Key = 1, Name = "Entry number 1" });
            //grp1.Entries.Add(new Entry() { Key = 2, Name = "Entry number 2" });
            //grp1.Entries.Add(new Entry() { Key = 3, Name = "Entry number 3" });

            ////grp2
            //grp2.Entries.Add(new Entry() { Key = 4, Name = "Entry number 4" });
            //grp2.Entries.Add(new Entry() { Key = 5, Name = "Entry number 5" });
            //grp2.Entries.Add(new Entry() { Key = 6, Name = "Entry number 6" });

            ////grp3
            //grp3.Entries.Add(new Entry() { Key = 7, Name = "Entry number 7" });
            //grp3.Entries.Add(new Entry() { Key = 8, Name = "Entry number 8" });
            //grp3.Entries.Add(new Entry() { Key = 9, Name = "Entry number 9" });

            ////grp4
            //grp4.Entries.Add(new Entry() { Key = 10, Name = "Entry number 10" });
            //grp4.Entries.Add(new Entry() { Key = 11, Name = "Entry number 11" });
            //grp4.Entries.Add(new Entry() { Key = 12, Name = "Entry number 12" });

            //grp4.SubGroups.Add(grp1);
            //grp2.SubGroups.Add(grp4);

            //Groups.Add(grp1);
            //Groups.Add(grp2);
            //Groups.Add(grp3);

            //RootGroup.SubGroups = Groups;
            return RootGroups;
        }


        // TODO: syncrhonize content with TreeStructure objects
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
