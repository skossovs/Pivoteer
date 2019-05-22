using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PivoteerWPF.Common
{
    // TODO: Rename this entity
    class ProjectFile<T>
        where T: new()
    {
        private string path;
        private string fileName;
        private T      content;
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
            content = new T();
            if(firstTime)
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.InitialDirectory = path;
                openFileDialog.FileName         = fileName;

                if(openFileDialog.ShowDialog() == true)
                {
                    fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    path     = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                    firstTime = false;
                }
            }
            ApplicationCommands.SaveJsonObject(FullPath, content); 
        }
        public void Load()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = path;

            if (openFileDialog.ShowDialog() == true)
            {
                fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                path = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
            }
            // TODO: Load object
        }
        public void Save()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = path;

            if (openFileDialog.ShowDialog() == true)
            {
                fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                path = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
            }

            ApplicationCommands.SaveJsonObject(FullPath, content);
        }
    }
}