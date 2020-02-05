using System.Windows.Controls;

namespace MyExplorer.ViewModel
{
    internal class SettingWindow : BaseVM
    {
        public SettingWindow()
        {
            Services.Navigator.NewFrame += (c) => { Pages = new Frame() { Content = c }; };
            Services.Navigator.SetFrame(Services.Navigator.FrameName.Main);
            Statusbar = new Frame() { Content = new Controls.StatusBar(new StatusBar()) };
        }

        Frame _pages;
        public Frame Pages
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
