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

        public T Content { get { return content; } }

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
            ApplicationCommands.SaveJsonObject(FullPath, content); 
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
            // TODO: too complicated, get rid of types.
            content = (T) ApplicationCommands.ReadJsonObject(FullPath, typeof(T));
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

            ApplicationCommands.SaveJsonObject(FullPath, content);
        }
    }
}