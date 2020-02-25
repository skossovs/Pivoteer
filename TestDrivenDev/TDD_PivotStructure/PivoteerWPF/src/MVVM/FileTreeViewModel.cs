using PivoteerWPF.Common;
using PivoteerWPF.Data;
using PivoteerWPF.MVVM.Messages;
using PivoteerWPF.src.Common;
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
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<ExcelFileCommandMessage>(this, ReceiveExcelFileCommand);
        }

        private void ReceiveExcelFileCommand(ExcelFileCommandMessage fileCommand)
        {
            switch(fileCommand.Command)
            {
                case TreeNodeCommand.Add:
                    var sheets = ApplicationCommands.RetreiveSheets(fileCommand.FullPath);
                    // Add to Project
                    _content.ExcelFiles.Add(new ExcelFileData()
                    {
                        ExcelFileFullPath = fileCommand.FullPath,
                        Sheets = sheets.Select(s => new SheetData() {SheetName = s, ClassName = string.Empty }).ToList()
                    });
                    // Update TreeView
                    TreeData = ConvertFromProjectToGroups();
                    break;
            }
        }

        private void ReceiveFileCommand(FileCommandMessage fileCommand)
        {
            switch(fileCommand.Command)
            {
                case "NEW":
                    _content = new Project();
                    ApplicationCommands.SaveJsonObject(fileCommand.Path, _content);
                    TreeData = ConvertFromProjectToGroups();
                    break;
                case "OPEN":
                    _content = (Project) ApplicationCommands.ReadJsonObject(fileCommand.Path, typeof(Project));
                    if(_content != null)
                        TreeData = ConvertFromProjectToGroups();
                    break;
                case "SAVE":
                    ApplicationCommands.SaveJsonObject(fileCommand.Path, _content);
                    break;
                default:
                    throw new Exception($"Unknnown File Menu command {fileCommand.Command}");
            }
        }

        public List<Group> ConvertFromProjectToGroups()
        {
            // TODO: overall ugly
            // TODO: not the best place to send the message. Decouple
            List<TreeNode> lstNodes = new List<TreeNode>();

            // Name
            var RootGroup = new Group() {
                Key  = _content.ProjectDescription.GetHashCode(),
                Name = _content.ProjectDescription,
                Path = _content.ProjectDescription,
                SubGroups = new List<Group>(),
                Entries = new List<Entry>()
            };

            var properties = new Dictionary<string, string>();
            properties.Add("ProjectName", _content.ProjectDescription);

            lstNodes.Add(new TreeNode()
            {
                Type = TreeNodeType.Root,
                Key = _content.ProjectDescription.GetHashCode(),
                Properties = properties
            });

            var RootGroups = new List<Group>() { RootGroup };
            var parentPath = RootGroup.Path;

            _content?.ExcelFiles?.ForEach(f =>
            {
                var g = f.ExcelFileDataToGroup(parentPath);
                RootGroup.SubGroups.Add(g);

                properties = new Dictionary<string, string>();
                properties.Add(ExcelContentConstants.ExcelFilePathNodeName, f.ExcelFileFullPath);

                lstNodes.Add(new TreeNode()
                {
                    Type = TreeNodeType.ExcelFile,
                    Key = g.Key,
                    Properties = properties
                });

                f?.Sheets?.ForEach(s =>
                {
                    g.Entries.Add(new Entry()
                    {
                        Name = s.SheetName,
                        Path = g.Path + (char)0x00 + s.SheetName,
                        Key = (g.Path + (char)0x00 + s.SheetName).GetHashCode()
                    });

                    properties = new Dictionary<string, string>();
                    properties.Add(ExcelContentConstants.ExcelFilePathNodeName, f.ExcelFileFullPath);
                    properties.Add(ExcelContentConstants.SheetNameNode, s.SheetName);
                    properties.Add(ExcelContentConstants.ClassNameNode, s.ClassName);

                    lstNodes.Add(new TreeNode()
                    {
                        Type = TreeNodeType.ExcelSheet,
                        Key = (g.Path + (char)0x00 + s.SheetName).GetHashCode(),
                        Properties = properties
                    });
                });
            });

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new TreeViewPopulatedMessage(lstNodes));
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
