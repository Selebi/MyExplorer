using MyExplorer.Services;
using System.Collections.ObjectModel;

namespace MyExplorer.ViewModel
{
    internal class HotKeys : BaseVM
    {
        public HotKeys()
        {
            Settings.GetInstance().HotKeys.ForEach(k =>
            {
                ForbiddenHotkeys.Add(new Controls.HotKey(new HotKey(k)));
            });

            Settings.GetInstance().HotkeysChanged += l => {
                ForbiddenHotkeys.Clear();
                l.ForEach(k =>
                {
                    ForbiddenHotkeys.Add(new Controls.HotKey(new HotKey(k)));
                });
            };
        }

        public RelayCommand Back
        {
            get => new RelayCommand((o) =>
            {
                Navigator.GetInstance(Navigator.WindowName.Settings).SetPreviousFrame();
            });
        }

        public RelayCommand Add
        {
            get => new RelayCommand((o) =>
            {
                Model.HotkeyProcessor.locked = true;
                Navigator.GetInstance(Navigator.WindowName.Settings).ShowMessage(Navigator.FrameName.AddHotKey);
            });
        }

        ObservableCollection<Controls.HotKey> forbiddenHotkeys = new ObservableCollection<Controls.HotKey>();
        public ObservableCollection<Controls.HotKey> ForbiddenHotkeys
        {
            get => forbiddenHotkeys;
            set
            {
                forbiddenHotkeys = value;
                OnPropertyChanged();
            }
        }
    }
}
