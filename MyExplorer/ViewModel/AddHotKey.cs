using MyExplorer.Services;
using System.Linq;

namespace MyExplorer.ViewModel
{
    internal class AddHotKey : BaseVM
    {
        public AddHotKey()
        {
            Model.HotkeyProcessor.CurrentHotKeyChanged += HotkeyProcessor_CurrentHotKeyChanged;
        }

        private void HotkeyProcessor_CurrentHotKeyChanged(string hotkeystr)
        {
            Hotkey = hotkeystr;
        }

        public RelayCommand Cancel
        {
            get => new RelayCommand((o) =>
            {
                hotkey = "";
                Model.HotkeyProcessor.RemoveAllKeys();
                Model.HotkeyProcessor.LockedAll = false;
                Navigator.GetInstance(Navigator.WindowName.Settings).CloseMessage();
            });
        }
        public RelayCommand Add
        {
            get => new RelayCommand((o) =>
            {
                Settings.GetInstance().AddHotkey(hotkey);
                hotkey = "";
                Model.HotkeyProcessor.RemoveAllKeys();
                Model.HotkeyProcessor.LockedAll = false;
                Navigator.GetInstance(Navigator.WindowName.Settings).CloseMessage();
            },
                o => { return hotkey != "" && !Settings.GetInstance().HotKeys.Any(h => { return h == hotkey; } ); });
        }

        public RelayCommand Clean
        {
            get => new RelayCommand((o) =>
            {
                hotkey = "";
                Model.HotkeyProcessor.RemoveAllKeys();
            });
        }

        string hotkey = "";
        public string Hotkey
        {
            get => hotkey;
            set
            {
                if (!hotkey.Contains(value))
                {
                    hotkey = value;
                }
                OnPropertyChanged();
            }
        }
    }
}
