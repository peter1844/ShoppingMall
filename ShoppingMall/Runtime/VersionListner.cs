using ShoppingMall.Helper;
using System;
using System.IO;

namespace ShoppingMall.Runtime
{
    public static class VersionListner
    {
        private static FileSystemWatcher fileSystemWatcher;

        public static void Initialize()
        {
            if (fileSystemWatcher != null)
            {
                return;
            }

            fileSystemWatcher = new FileSystemWatcher
            {
                Path = AppDomain.CurrentDomain.BaseDirectory,
                Filter = "*.json",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
            };

            fileSystemWatcher.Changed += OnChanged;

            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            ConfigurationsHelper.LoadVersion();
            LogHelper.logger.Info($"file is {e.ChangeType}: {e.FullPath}");
        }

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
