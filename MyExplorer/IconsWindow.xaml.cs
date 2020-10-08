using System.Windows;

namespace MyExplorer
{
    public partial class IconsWindow : Window
    {
        public IconsWindow(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
