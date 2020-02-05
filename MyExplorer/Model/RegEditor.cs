using Microsoft.Win32;
using System;

namespace MyExplorer.Model
{
    internal static class RegEditor
    {
        static bool _explorerRegistred;
        public static bool ExplorerRegistred { get => _explorerRegistred; private set { _explorerRegistred = value; RegistredChanged?.Invoke(value); } }
        public static event Action<bool> RegistredChanged;

        public static void RegSintekExplorer()
        {
            var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey key = view64.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            key.SetValue("Shell", "MyExplorer.exe", RegistryValueKind.String);
            view64.Close();
            key.Close();
            ExplorerRegistred = true;
        }

        public static void RegWindowsExplorer()
        {
            var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey key = view64.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            key.SetValue("Shell", "Explorer.exe", RegistryValueKind.String);
            view64.Close();
            key.Close();
            ExplorerRegistred = false;
        }

        public static bool IsSintekExplorerRegistred()
        {
            var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey key = view64.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            ExplorerRegistred = ((string)key.GetValue("Shell")).ToLower() == "myexplorer.exe";
            view64.Close();
            key.Close();
            return ExplorerRegistred;
        }
    }
}
