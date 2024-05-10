using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Cybercraft.Common.WinForms
{
    public class RecentlyUsedFiles
    {
        private readonly string applicationKey;
        private readonly int maxFilesToKeep;

        public RecentlyUsedFiles(string applicationKey, int maxFilesToKeep)
        {
            this.maxFilesToKeep = maxFilesToKeep;
            this.applicationKey = "Software\\Cybercraft\\" + applicationKey + "\\Recent File List";
            using (Registry.CurrentUser.CreateSubKey(this.applicationKey))
            {
            }
        }

        private static readonly FileValueNamePair[] EmptyNamePairs = new FileValueNamePair[0];

        public string[] RecentFiles
        {
            get
            {
                try
                {
                    using (var key = Registry.CurrentUser.OpenSubKey(applicationKey))
                    {
                        return GetRecentFiles(key).Select(f => f.FileName).Take(maxFilesToKeep).ToArray();
                    }
                }
                // ReSharper disable once RedundantCatchClause
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private class FileValueNamePair
        {
            public readonly string KeyName;
            public readonly int KeyNumber;
            public readonly string FileName;

            public FileValueNamePair(string keyName, int keyNumber, string fileName)
            {
                KeyName = keyName;
                KeyNumber = keyNumber;
                FileName = fileName;
            }
        }

        private static FileValueNamePair[] GetRecentFiles(RegistryKey key)
        {
            if (key == null)
                return EmptyNamePairs;
            var valueNames = key.GetValueNames().Where(k => k.StartsWith("File")).ToArray();
            var files = new List<FileValueNamePair>();
            foreach (var valueName in valueNames)
            {
                object value = key.GetValue(valueName);
                if (value is string)
                {
                    if (int.TryParse(valueName.Substring(4), out var fileNumber))
                    {
                        files.Add(new FileValueNamePair(valueName, fileNumber, (string)value));
                    }
                }
            }
            return files.OrderBy(f => f.KeyNumber).ToArray();
        }

        public void Add(string fileName)
        {
            try
            {
                using (var key = Registry.CurrentUser.CreateSubKey(applicationKey))
                {
                    if (key == null)
                        return;
                    var files = GetRecentFiles(key).ToList();
                    files.ForEach(f => key.DeleteValue(f.KeyName));
                    var existingFile =
                        files.FirstOrDefault(f => string.Equals(f.FileName, fileName, StringComparison.InvariantCultureIgnoreCase));
                    if (existingFile != null)
                        files.Remove(existingFile);
                    files.Insert(0, new FileValueNamePair("", 0, fileName));

                    int counter = 1;
                    foreach (var file in files.Take(maxFilesToKeep))
                    {
                        key.SetValue("File" + counter, file.FileName);
                        counter++;
                    }
                }
            }
            // ReSharper disable once RedundantCatchClause
            catch (Exception)
            {
                throw;
            }
        }

        public void Remove(string fileName)
        {
            try
            {
                using (var key = Registry.CurrentUser.CreateSubKey(applicationKey))
                {
                    if (key == null)
                        return;
                    var files = GetRecentFiles(key).ToList();
                    files.ForEach(f => key.DeleteValue(f.KeyName));
                    var existingFile =
                        files.FirstOrDefault(f => string.Equals(f.FileName, fileName, StringComparison.InvariantCultureIgnoreCase));
                    if (existingFile != null)
                        files.Remove(existingFile);

                    int counter = 1;
                    foreach (var file in files.Take(maxFilesToKeep))
                    {
                        key.SetValue("File" + counter, file.FileName);
                        counter++;
                    }
                }
            }
            // ReSharper disable once RedundantCatchClause
            catch (Exception)
            {
                throw;
            }
        }

#if !NETCOREAPP
        public void PopulateMenu(MenuItem menu, EventHandler handler)
        {
            menu.MenuItems.Clear();
            var recentFiles = RecentFiles;
            foreach (var recentFile in recentFiles)
            {
                var menuItem = new MenuItem(recentFile, handler);
                menu.MenuItems.Add(menuItem);
            }
        }
#endif


    }
}
