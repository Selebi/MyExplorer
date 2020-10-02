using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public static void StartProcess(string file)
        {
            StartedProcesses.Add(file, Process.Start(file));
        }

        public static void StartExplorer()
        {
            if (RegEditor.IsSintekExplorerRegistred())
            {
                RegEditor.RegWindowsExplorer();
                Process.Start(@"C:\Windows\explorer.exe");
                Thread.Sleep(500); // Костыли, переписать!
                RegEditor.RegSintekExplorer();
            }
            else
                Process.Start(@"C:\Windows\explorer.exe");
        }

        public static Dictionary<string, Process> StartedProcesses { get; private set; } = new Dictionary<string, Process>();
    }
}
