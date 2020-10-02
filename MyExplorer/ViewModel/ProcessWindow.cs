using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Process.GetInstance().Actions = new ObservableCollection<Action>(settings.Actions);
            Process.GetInstance().Start();
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
    }
}
