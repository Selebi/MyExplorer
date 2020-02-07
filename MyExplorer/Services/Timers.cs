using System;
using System.Threading;

namespace MyExplorer.Services
{
    public static class Timers
    {
        public static event Action<bool> RegExplorerChange;
        public static void StartRegExplorerTimer(int interval)
        {
            bool state = Model.RegEditor.IsSintekExplorerRegistred();
            RegExplorerChange?.Invoke(state);
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(interval);
                    bool buf = Model.RegEditor.IsSintekExplorerRegistred();
                    if (state != buf)
                    {
                        state = buf;
                        RegExplorerChange?.Invoke(state);
                    }
                }
            })
            { IsBackground = true }.Start();
        }

        public static event Action<bool> WinExplorerChange;
        public static void StartWinExplorerTimer(int interval)
        {
            bool state = Model.ProcessWorker.IsProcessWork("explorer");
            WinExplorerChange?.Invoke(state);
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(interval);
                    bool buf = Model.ProcessWorker.IsProcessWork("explorer");
                    if (state != buf)
                    {
                        state = buf;
                        WinExplorerChange?.Invoke(state);
                    }
                }
            })
            { IsBackground = true }.Start();
        }
    }
}
