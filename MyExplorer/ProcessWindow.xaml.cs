using MyExplorer.Enums;
using MyExplorer.Services;
using System.Windows;

namespace MyExplorer
{
    public partial class ProcessWindow : Window
    {
        Navigator navigator;

        public ProcessWindow(object ViewModel)
        {
            InitializeComponent();
            if (MyExplorer.ViewModel.Settings.GetInstance().ShowSplash == false)
                Visibility = Visibility.Hidden;
            DataContext = ViewModel;
            Loaded += ProcessWindow_Loaded;
        }

        private void ProcessWindow_Loaded(object sender, RoutedEventArgs e)
        {
            navigator = Navigator.GetInstance(WindowName.Process);
            navigator.AddContainer(ContainerType.Main, Container);
            navigator.SetFrame(FrameName.Process, ContainerType.Main);
        }
    }
}
