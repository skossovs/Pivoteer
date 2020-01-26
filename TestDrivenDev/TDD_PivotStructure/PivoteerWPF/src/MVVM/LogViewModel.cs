using System;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace PivoteerWPF.MVVM
{
    class LogViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private string _logText;
        public LogViewModel()
        {
            FileWatherConfigure();
        }

        #region file watcher
        private FileSystemWatcher fileWatcher = new FileSystemWatcher();
        private string _latestLogFileName;
        private readonly string   filePattern = @"Pivot.Accessories*.log";

        public void FileWatherConfigure()
        {

            _latestLogFileName = GetLatestWritenFileFileInDirectory(new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory), filePattern).Name;
            fileWatcher.Path = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            fileWatcher.Filter = System.IO.Path.GetFileName(_latestLogFileName);
            fileWatcher.Changed += FileWatcher_Changed;
            fileWatcher.EnableRaisingEvents = true;
        }


        private static FileInfo GetLatestWritenFileFileInDirectory(DirectoryInfo directoryInfo, string pattern)
        {
            if (directoryInfo == null || !directoryInfo.Exists)
                return null;

            FileInfo[] files = directoryInfo.GetFiles(pattern);
            DateTime lastWrite = DateTime.MinValue;
            FileInfo lastWritenFile = null;

            foreach (FileInfo file in files)
            {
                if (file.LastWriteTime > lastWrite)
                {
                    lastWrite = file.LastWriteTime;
                    lastWritenFile = file;
                }
            }
            return lastWritenFile;
        }

        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            ReadFromTxt();
        }
        private void ReadFromTxt()
        {
            string[] lines = System.IO.File.ReadAllLines(_latestLogFileName);
            LogText = string.Join(Environment.NewLine, lines);
        }
        #endregion

        public string LogText
        {
            get
            {
                return _logText;
            }
            set
            {
                _logText = value;
                OnPropertyChanged("LogText");
            }
        }
    }
}
