﻿namespace MyExplorer.ViewModel
{
    public class Main : BaseVM
    {
        public Main()
        {
            _regButtonState = Model.RegEditor.ExplorerRegistred;
            _startStopExplorerButtonState = Model.ProcessWorker.IsProcessWork("explorer");

            Services.Timers.RegExplorerChange += s => { RegButtonState = s; };
            Services.Timers.WinExplorerChange += s => { StartStopExplorerButtonState = s; };
            Model.HotkeyProcessor.LockedStatusChanged += s => { StartStopBlockHotkeysState = s; };
            Model.RegEditor.SecurityException += () =>
            {
                Services.Navigator.GetInstance(Enums.WindowName.Settings).ShowModalMessage(Enums.ContainerType.Main, Enums.MessageType.Error, "Ошибка доступа к реестру", "Пользователь не имеет прав для чтения и записи реестра.");
            };
        }

        public string HelloText
        {
            get => Properties.Resources.Help;
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
                Services.Navigator.GetInstance(Enums.WindowName.Settings).SetFrame(Enums.FrameName.Settings, Enums.ContainerType.Main);
            });
        }
        public RelayCommand OpenHotKeys
        {
            get => new RelayCommand(o =>
            {
                Services.Navigator.GetInstance(Enums.WindowName.Settings).SetFrame(Enums.FrameName.Hotkeys, Enums.ContainerType.Main);
            });
        }

        public RelayCommand OpenUsers
        {
            get => new RelayCommand(o =>
            {
                Services.Navigator.GetInstance(Enums.WindowName.Settings).SetFrame(Enums.FrameName.Users, Enums.ContainerType.Main);
            });
        }

        public RelayCommand OpenActions
        {
            get => new RelayCommand(o =>
            {
                Services.Navigator.GetInstance(Enums.WindowName.Settings).SetFrame(Enums.FrameName.Actions, Enums.ContainerType.Main);
            });
        }

        public RelayCommand OpenJournal
        {
            get => new RelayCommand(o =>
            {
                Services.Navigator.GetInstance(Enums.WindowName.Settings).SetFrame(Enums.FrameName.Journal, Enums.ContainerType.Main);
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
