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

        private void roll_up(object sender, RoutedEventArgs e)
        {
            if (this.Height > 200)
            {
                this.Height = 96;
                roll_text.Text = "Развернуть";
            }
            else
            {
                this.Height = 500;
                roll_text.Text = "Свернуть";
            }
        }
    }
}
