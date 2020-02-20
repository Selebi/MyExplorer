namespace MyExplorer.ViewModel
{
    internal class StatusBar : BaseVM
    {
        public StatusBar()
        {
            WinExplorerStatus = Model.ProcessWorker.IsProcessWork("explorer");
            RegStatus = Model.RegEditor.IsSintekExplorerRegistred();

            Services.Timers.RegExplorerChange += b => { RegStatus = b; };
            Services.Timers.WinExplorerChange += b => { WinExplorerStatus = b; };
            Model.HotkeyProcessor.LockedStatusChanged += b => { HotkeyLockStatus = b; };
        }

        bool _regstatus;
        public bool RegStatus
        {
            get => _regstatus;
            set
            {
                _regstatus = value;
                OnPropertyChanged();
            }
        }

        bool _winExplorerStatus;
        public bool WinExplorerStatus
        {
            get => _winExplorerStatus;
            set
            {
                _winExplorerStatus = value;
                OnPropertyChanged();
            }
        }

        bool _hotkeyLockStatus = true;
        public bool HotkeyLockStatus
        {
            get => _hotkeyLockStatus;
            set
            {
                _hotkeyLockStatus = value;
                OnPropertyChanged();
            }
        }
    }
}
