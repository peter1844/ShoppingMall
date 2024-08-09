using ShoppingMall.Helper;
using System;
using System.IO;

namespace ShoppingMall.Runtime
{
    public static class VersionListner
    {
        private static FileSystemWatcher fileSystemWatcher;

        /// <summary>
        /// 初始化Version.json檔的監聽
        /// </summary>
        public static void Initialize()
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

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            ConfigurationsHelper.LoadVersion();
            LogHelper.Info($"file is {e.ChangeType}: {e.FullPath}");
        }

        /// <summary>
        /// 釋放監聽資源
        /// </summary>
        public static void Dispose()
        {
            if (fileSystemWatcher != null)
            {
                fileSystemWatcher.Dispose();
                fileSystemWatcher = null;
            }
        }
    }
}
