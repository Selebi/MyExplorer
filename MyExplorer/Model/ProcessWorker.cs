using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.Model
{
    public static class ProcessWorker
    {
        public static bool IsProcessWork(string name)
        {
            return new List<Process>(Process.GetProcesses()).Any(p => p.ProcessName == name);
        }

        public static void KillProcess(string name)
        {
            new List<Process>(Process.GetProcessesByName(name)).ForEach(p => { p.Kill(); });
        }

        public static void StartProcess(string file)
        {
            Process.Start(file);
        }
    }
}
