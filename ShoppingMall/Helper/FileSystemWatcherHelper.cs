using System;
using System.IO;

namespace ShoppingMall.Helper
{
    public static class FileSystemWatcherHelper
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

            fileSystemWatcher.Created += OnChanged;
            fileSystemWatcher.Changed += OnChanged;
            fileSystemWatcher.Deleted += OnChanged;
            fileSystemWatcher.Renamed += OnRenamed;

            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            ConfigurationsHelper.LoadVersion();
            LogHelper.logger.Info($"file is {e.ChangeType}: {e.FullPath}");
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            LogHelper.logger.Info("file rename");
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
