using MyExplorer.Properties;
using System;
using System.Drawing;

namespace MyExplorer.ViewModel
{
    internal class Main : BaseVM
    {
        public Main()
        {
            _regButtonState = Model.RegEditor.ExplorerRegistred;
            _startStopExplorerButtonState = Model.ProcessWorker.IsProcessWork("explorer");

            Services.Timers.RegExplorerChange += s => { RegButtonState = s; };
            Services.Timers.WinExplorerChange += s => { StartStopExplorerButtonState = s; };
            Model.HotkeyProcessor.LockedStatusChanged += s => { StartStopBlockHotkeysState = s; };
        }

        string _helloText = "Место для вашей рекламы!";
        public string HelloText
        {
            get => _helloText;
            set
            {
                _helloText = value;
                OnPropertyChanged();
            }
        }

        bool _regButtonState;
        public bool RegButtonState
        {
            get => _regButtonState;
            set
            {
                _regButtonState = value;
                OnPropertyChanged();
            }
        }

        bool _startStopExplorerButtonState;
        public bool StartStopExplorerButtonState
        {
            get => _startStopExplorerButtonState;
            set
            {
                _startStopExplorerButtonState = value;
                OnPropertyChanged();
            }
        }

        bool _startStopBlockHotkeysState = true;
        public bool StartStopBlockHotkeysState
        {
            get => _startStopBlockHotkeysState;
            set
            {
                _startStopBlockHotkeysState = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand OpenSettings
        {
            get => new RelayCommand(o =>
            {
                Services.Navigator.GetInstance(Services.Navigator.WindowName.Settings).SetFrame(Services.Navigator.FrameName.Settings);
            });
        }
        public RelayCommand OpenHotKeys
        {
            get => new RelayCommand(o =>
            {
                Services.Navigator.GetInstance(Services.Navigator.WindowName.Settings).SetFrame(Services.Navigator.FrameName.Hotkeys);
            });
        }

        public RelayCommand OpenUsers
        { 
            get => new RelayCommand(o =>
            {
                Services.Navigator.GetInstance(Services.Navigator.WindowName.Settings).SetFrame(Services.Navigator.FrameName.Users);
            });
        }

        public RelayCommand OpenJournal
        {
            get => new RelayCommand(o =>
            {
                Services.Navigator.GetInstance(Services.Navigator.WindowName.Settings).SetFrame(Services.Navigator.FrameName.Journal);
            });
        }

        public RelayCommand RegExplorer
        {
            get => new RelayCommand(o =>
            {
                if (Model.RegEditor.ExplorerRegistred)
                {
                    Model.RegEditor.RegWindowsExplorer();
                }
                else
                {
                    Model.RegEditor.RegSintekExplorer();
                }
            });
        }

        public RelayCommand StartStopExplorer
        {
            get => new RelayCommand(o =>
            {
                if (Model.ProcessWorker.IsProcessWork("explorer"))
                {
                    Model.ProcessWorker.KillProcess("explorer");
                }
                else
                {
                    Model.ProcessWorker.StartExplorer();
                }
            });
        }

        public RelayCommand StartStopBlockHotkeys
        {
            get => new RelayCommand(o =>
            {
                if (Model.HotkeyProcessor.Locked)
                {
                    Model.HotkeyProcessor.Locked = false;
                }
                else
                {
                    Model.HotkeyProcessor.Locked = true;
                }
            });
        }
    }
}
