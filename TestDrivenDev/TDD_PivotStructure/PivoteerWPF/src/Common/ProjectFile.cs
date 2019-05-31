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
            path = System.IO.Directory.GetCurrentDirectory();
            fileName = "new test.json";
            firstTime = true;
        }

        public void CreateNew()
        {
            if(firstTime)
            {
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
        public void Load()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = path;
            openFileDialog.AddExtension = true;
            openFileDialog.DefaultExt = "json";

            if (openFileDialog.ShowDialog() == true)
            {
                fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                path = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
            }
        }
        public void Save()
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.InitialDirectory = path;
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = "json"; // TODO: extension filter is not working

            if (saveFileDialog.ShowDialog() == true)
            {
                fileName = System.IO.Path.GetFileName(saveFileDialog.FileName);
                path = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
            }
        }
    }
}