using ShoppingMall.Helper;
using ShoppingMall.Interface;
using System;
using System.IO;

namespace ShoppingMall.Runtime
{
    public class VersionListner : IVersionListner
    {
        private IConfigurationsHelper _configurationsHelper;
        private ILogHelper _logHelper;
        private FileSystemWatcher fileSystemWatcher;

        public VersionListner(IConfigurationsHelper configurationsHelper = null, ILogHelper logHelper = null)
        {
            _configurationsHelper = configurationsHelper ?? new ConfigurationsHelper();
            _logHelper = logHelper ?? new LogHelper();
        }

        /// <summary>
        /// 初始化Version.json檔的監聽
        /// </summary>
        public void Initialize()
        {
            if (fileSystemWatcher != null)
            {
                return;
            }

            fileSystemWatcher = new FileSystemWatcher
            {
                Path = AppDomain.CurrentDomain.BaseDirectory,
                Filter = "Version.json",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
            };

            fileSystemWatcher.Changed += OnChanged;

            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            _configurationsHelper.LoadVersion();
            _logHelper.Info($"file is {e.ChangeType}: {e.FullPath}");
        }

        /// <summary>
        /// 釋放監聽資源
        /// </summary>
        public void Dispose()
        {
            if (fileSystemWatcher != null)
            {
                fileSystemWatcher.Dispose();
                fileSystemWatcher = null;
            }
        }
    }
}
