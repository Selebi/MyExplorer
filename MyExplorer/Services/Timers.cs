using System;
using System.Threading;

namespace MyExplorer.Services
{
    public static class Timers
    {
        static bool _timerFlag;

        public static event Action<bool> RegExplorerChange;

        public static void StopTimers()
        {
            _timerFlag = false;
        }

        public static void StartRegExplorerTimer(int interval)
        {
            if (Model.RegEditor.CanUseRegistry())
            {
                bool state = Model.RegEditor.IsSintekExplorerRegistred();
                _timerFlag = true;
                RegExplorerChange?.Invoke(state);
                new Thread(() =>
                {
                    while (_timerFlag)
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
        }

        public static event Action<bool> WinExplorerChange;
        public static void StartWinExplorerTimer(int interval)
        {
            bool state = Model.ProcessWorker.IsProcessWork("explorer");
            _timerFlag = true;
            WinExplorerChange?.Invoke(state);
            new Thread(() =>
            {
                while (_timerFlag)
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
