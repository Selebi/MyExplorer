using MyExplorer.Services;
using System.Windows;

namespace MyExplorer
{
    public partial class IconsWindow : Window
    {
        Navigator navigator;
        public IconsWindow(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
            Loaded += IconsWindow_Loaded;
        }

        private void IconsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            navigator = Navigator.GetInstance(Enums.WindowName.Icons);
            navigator.AddContainer(Enums.ContainerType.Main, Container);
            navigator.SetFrame(Enums.FrameName.Icons, Enums.ContainerType.Main);
        }
    }
}
