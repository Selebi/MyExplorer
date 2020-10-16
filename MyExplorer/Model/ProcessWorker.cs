using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

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
            if (StartedProcesses.ContainsKey(file))
            {
                Process.Start(file, param);
                return "Копия";
            }
            else
            {
                StartedProcesses.Add(file, Process.Start(file, param));
            }
            return "";
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
