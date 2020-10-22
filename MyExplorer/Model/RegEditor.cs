using Microsoft.Win32;
using System;
using System.IO;

namespace MyExplorer.Model
{
    public static class RegEditor
    {
        static Services.FileLogger fl = Services.FileLogger.GetInstance(ViewModel.Settings.GetInstance().LogFile);
        static bool _explorerRegistred;
        public static bool ExplorerRegistred { get => _explorerRegistred; private set { _explorerRegistred = value; RegistredChanged?.Invoke(value); } }
        public static event Action<bool> RegistredChanged;
        public static event Action SecurityException;

        public static bool CanUseRegistry()
        {
            try
            {
                var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey key = view64.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            }
            catch (System.Security.SecurityException)
            {
                return false;
            }
            return true;
        }

        public static void RegSintekExplorer()
        {
            try
            {
                var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey key = view64.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
                key.SetValue("Shell", $"{Directory.GetCurrentDirectory()}\\Sintek Explorer.exe", RegistryValueKind.String);
                view64.Close();
                key.Close();
                ExplorerRegistred = true;
                File.WriteAllText($"{Environment.SystemDirectory}\\SintekExplorerPath", Directory.GetCurrentDirectory());
                fl.Write($"SintekExplorerPath установлен - \"{Directory.GetCurrentDirectory()}\"", Enums.LogType.Info);
            }
            catch (System.Security.SecurityException)
            {
                fl.Write("У пользователя нет прав на изменение реестра.", Enums.LogType.Info);
                SecurityException?.Invoke();
            }
        }

        public static void RegWindowsExplorer()
        {
            try
            {
                var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey key = view64.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
                key.SetValue("Shell", "Explorer.exe", RegistryValueKind.String);
                view64.Close();
                key.Close();
                ExplorerRegistred = false;
            }
            catch (System.Security.SecurityException)
            {
                fl.Write("У пользователя нет прав на изменение реестра.", Enums.LogType.Info);
                SecurityException?.Invoke();
            }
        }

        public static bool IsSintekExplorerRegistred()
        {
            try
            {
                var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey key = view64.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
                ExplorerRegistred = ((string)key.GetValue("Shell")).Contains("Sintek");
                view64.Close();
                key.Close();
            }
            catch (System.Security.SecurityException)
            {
                fl.Write("У пользователя нет прав на чтение реестра.", Enums.LogType.Info);
                SecurityException?.Invoke();
            }
            return ExplorerRegistred;
        }
    }
}
