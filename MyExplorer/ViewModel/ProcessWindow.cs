using MyExplorer.Enums;
using System.Windows;
using System.Windows.Controls;

namespace MyExplorer.ViewModel
{
    public class ProcessWindow : BaseVM
    {
        Settings settings;
        public ProcessWindow()
        {
            //settings = Settings.GetInstance();

            //Process.GetInstance().Done += ProcessWindow_Done;
            //Process.GetInstance().Start(settings.Actions);

        }

        public void Loaded(object sender, object e)
        {

        }

        private void ProcessWindow_Done()
        {
            Visibility = Visibility.Collapsed;
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
