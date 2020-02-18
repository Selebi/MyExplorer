using System.Windows.Controls;

namespace MyExplorer.ViewModel
{
    internal class SettingWindow : BaseVM
    {
        public SettingWindow()
        {
            Services.Navigator.CreateInstance(Services.Navigator.WindowName.Settings, Pages);
            Services.Navigator.GetInstance(Services.Navigator.WindowName.Settings).SetFrame(Services.Navigator.FrameName.Main);
            Statusbar = new Frame() { Content = new Controls.StatusBar(new StatusBar()) };
        }

        Grid _pages = new Grid();
        public Grid Pages
        {
            get => _pages;
            set
            {
                _pages = value;
                OnPropertyChanged();
            }
        }

        Frame _statusbar;
        public Frame Statusbar
        {
            get => _statusbar;
            set
            {
                _statusbar = value;
                OnPropertyChanged();
            }
        }
    }
}
