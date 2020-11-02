using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Windows;

namespace MyExplorer.Model
{
    public static class ProcessWorker
    {
        public static bool IsProcessWork(string name)
        {
            return new List<Process>(Process.GetProcesses()).Any(p => p.ProcessName == name);
        }

        public static bool IsStartedProcessWork(string file)
        {
            if (StartedProcesses.TryGetValue(file, out Process process))
            {
                return IsProcessWork(process.ProcessName);
            }
            return false;
        }

        public static void KillProcess(string name)
        {
            new List<Process>(Process.GetProcessesByName(name)).ForEach(p => { p.Kill(); });
        }

        public static string StartProcess(string file, string param = "")
        {
            if (file == "")
                return $"Файл не указан";
            try
            {
                if (StartedProcesses.ContainsKey(file))
                {
                    Process.Start(file, param);
                    //return "Копия";
                }
                else
                {
                    StartedProcesses.Add(file, Process.Start(file, param));
                }
            }
            catch (Exception ex)
            {
                return $"{ex.Message} : {file}";
            }
            return "";
        }

        public static void StartExplorer()
        {
            if (RegEditor.IsSintekExplorerRegistred())
            {
                RegEditor.RegWindowsExplorer();
                Process.Start(@"C:\Windows\explorer.exe");
                Thread.Sleep(1000); // Костыли, переписать!
                RegEditor.RegSintekExplorer();
            }
            else
                Process.Start(@"C:\Windows\explorer.exe");
        }

        public static Dictionary<string, Process> StartedProcesses { get; private set; } = new Dictionary<string, Process>();

        public static bool RunSExplorerAsAdmin()
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);

            if (hasAdministrativeRight == false)
            {
                ProcessStartInfo processInfo = new ProcessStartInfo(); //создаем новый процесс
                processInfo.Verb = "runas";
                processInfo.FileName = "Sintek Explorer.exe";
                try
                {
                    Process.Start(processInfo);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"SintekExplorerPath - \"{Directory.GetCurrentDirectory()}\". {ex.Message}", "Ошибка запроса привелегий", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        public static bool IsSExplorerAsAdmin()
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return pricipal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
