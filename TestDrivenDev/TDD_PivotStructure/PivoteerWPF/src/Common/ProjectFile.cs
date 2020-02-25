using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PivoteerWPF.Common
{
    // TODO: Rename this entity
    class ProjectFile
    {
        private string path;
        private string fileName;
        private bool   firstTime;

        public string FullPath
        {
            get
            {
                return System.IO.Path.Combine(path, fileName);
            }
        }

        public ProjectFile()
        {
            path = string.Empty;
            fileName = string.Empty;
            firstTime = true;
        }

        public void CreateNew()
        {
            if(firstTime)
            {
                path = System.IO.Directory.GetCurrentDirectory();
                fileName = "new test.json";

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();

                saveFileDialog.InitialDirectory = path;
                saveFileDialog.FileName         = fileName;
                saveFileDialog.AddExtension     = true;
                saveFileDialog.DefaultExt       = "json";

                if(saveFileDialog.ShowDialog() == true)
                {
                    fileName = System.IO.Path.GetFileName(saveFileDialog.FileName);
                    path     = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
                    firstTime = false;
                }
            }
        }
        public bool Load()
        {
            bool isCancelled = false;
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = path;
            openFileDialog.DefaultExt = "json";
            openFileDialog.Filter = "JSON documents (*.json)|*.json";
            openFileDialog.AddExtension = true;

            if (openFileDialog.ShowDialog() == true)
            {
                fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                path = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
            }
            else
                isCancelled = true;

            return isCancelled;
        }
        public bool Save()
        {
            bool isCancelled = false;
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.InitialDirectory = path;
            saveFileDialog.DefaultExt = "json";
            saveFileDialog.Filter = "JSON documents (*.json)|*.json";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                fileName = System.IO.Path.GetFileName(saveFileDialog.FileName);
                path = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
            }
            else
                isCancelled = true;

            return isCancelled;
        }
    }
}