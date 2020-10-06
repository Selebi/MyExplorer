using System.Windows;
using System.Windows.Controls;

namespace MyExplorer.ViewModel
{
    public class ProcessWindow : BaseVM
    {
        Settings settings;
        public ProcessWindow()
        {
            settings = Settings.GetInstance();
            Services.Navigator.CreateInstance(Services.Navigator.WindowName.Process, Pages);
            Services.Navigator.GetInstance(Services.Navigator.WindowName.Process).SetFrame(Services.Navigator.FrameName.Process);

            Process.GetInstance().Done += ProcessWindow_Done;
            Process.GetInstance().Start(settings.Actions);

        }

        private void ProcessWindow_Done()
        {
            Visibility = Visibility.Collapsed;
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

        Visibility _visibility;
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged();
            }
        }
    }
}
