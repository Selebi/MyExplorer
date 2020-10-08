using MyExplorer.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace MyExplorer
{
    public partial class IconsWindow : Window, IExplorerWindow
    {
        public Grid ContainerFrame { get => Container; }
        public IconsWindow(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
