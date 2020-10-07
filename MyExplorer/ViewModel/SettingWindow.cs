using MyExplorer.Enums;
using System.Windows.Controls;

namespace MyExplorer.ViewModel
{
    public class SettingWindow : BaseVM
    {
        public SettingWindow()
        {
            Services.Navigator.GetInstance(WindowName.Settings).SetFrame(FrameName.Main);
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
