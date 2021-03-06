﻿using PivoteerWPF.Common;
using PivoteerWPF.Data;
using PivoteerWPF.MVVM.Messages;
using PivoteerWPF.src.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PivoteerWPF.MVVM
{
    class TabItemsModel : INotifyPropertyChanged
    {
        IList<TreeNode>  _treeNodes;
        TreeNode         _treeNode;
        private readonly DelegateCommand<string> _addExcelFileCommand;
        private readonly DelegateCommand<string> _pivotCommandRun;
        private readonly DelegateCommand<string> _pivotCommandValidate;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public TabItemsModel()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<TreeViewSelectionMessage>(this, ReceiveTreeViewSelectionCommand);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<TreeViewPopulatedMessage>(this, ReceiveTreeViewPopulatedCommand);

            _addExcelFileCommand  = new DelegateCommand<string>(AddExcel);
            _pivotCommandRun      = new DelegateCommand<string>(Run);
            _pivotCommandValidate = new DelegateCommand<string>(Validate);
        }
        // TODO: implement Tab addition/deletion
        private void ReceiveTreeViewPopulatedCommand(TreeViewPopulatedMessage tvPopulatedMessage)
        {
            TreeNodes = tvPopulatedMessage.TreeNodes;
        }
        private void ReceiveTreeViewSelectionCommand(TreeViewSelectionMessage tvSelectedMessage)
        {
            SelectedNode = TreeNodes.First(t => t.Key == tvSelectedMessage.Key);
        }

        public IList<TreeNode> TreeNodes
        {
            get
            {
                return _treeNodes;
            }
            set
            {
                _treeNodes = value;
                OnPropertyChanged("TreeNodes");
            }
        }

        public TreeNode SelectedNode
        {
            get
            {
                return _treeNode;
            }
            set
            {
                _treeNode = value;
                OnPropertyChanged("SelectedNode");
            }
        }
        public string FileFullPath
        {
            get
            {
                return _treeNode.Properties[ExcelContentConstants.ExcelFilePathNodeName];
            }
        }

        public DelegateCommand<string> AddExcelFileCommand
        {
            get
            {
                return _addExcelFileCommand;
            }
        }

        public IEnumerable<string> PivotClasses
        {
            get
            {
                return ApplicationCommands.ExtractPivotClasses();
            }
        }

        private void AddExcel(string _)
        {
            var t = ApplicationCommands.RunOpenFileDialog(".xlsx", System.IO.Directory.GetCurrentDirectory());
            if(!t.Item1)
            {
                // Send message back to TreeView
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new ExcelFileCommandMessage(_treeNode.Key, TreeNodeCommand.Add, t.Item2));
            }
        }
        #region run / verify commands
        // TODO: rename
        public DelegateCommand<string> PivotCommandRun
        {
            get
            {
                return _pivotCommandRun;
            }
        }
        public DelegateCommand<string> PivotCommandValidate
        {
            get
            {
                return _pivotCommandValidate;
            }
        }

        private void Validate(string _)
        {
            var lstData = LoadFromExcel();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new PivotCommandMessage(_treeNode.Key, lstData, TreeNodeCommand.Validate));
        }
        private void Run(string _)
        {
            var lstData = LoadFromExcel();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new PivotCommandMessage(_treeNode.Key, lstData, TreeNodeCommand.Run));
        }

        private IEnumerable<PivotClassBase> LoadFromExcel()
        {
            IEnumerable<PivotClassBase> lstData = null;
            // Load from Excel
            var path      = _treeNode.Properties[ExcelContentConstants.ExcelFilePathNodeName];
            var sheet     = _treeNode.Properties[ExcelContentConstants.SheetNameNode];
            var className = _treeNode.Properties[ExcelContentConstants.ClassNameNode];

            switch (className)
            {
                case "OptionPrice":
                    throw new Exception("OptionPrice is not implemented yet, excel file is not populated.");
                    lstData = ExcelReadUtils<PivotClasses.Option>.RetrieveSheetData(path, sheet, ExcelContentConstants.ValueFieldName);
                    break;
                case "Stock":
                    lstData = ExcelReadUtils<PivotClasses.Stock>.RetrieveSheetData(path, sheet, ExcelContentConstants.ValueFieldName);
                    break;
                case "TwoByTwo":
                    lstData = ExcelReadUtils<PivotClasses.TwoByTwo>.RetrieveSheetData(path, sheet, ExcelContentConstants.ValueFieldName);
                    break;
                case "ThreeByTwo":
                    lstData = ExcelReadUtils<PivotClasses.ThreeByTwo>.RetrieveSheetData(path, sheet, ExcelContentConstants.ValueFieldName);
                    break;
                default:
                    throw new Exception($"{className} referenced class is not considered in switch-case");
            }

            return lstData;
        }
        #endregion
    }
}
