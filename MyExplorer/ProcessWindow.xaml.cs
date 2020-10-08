using MyExplorer.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace MyExplorer
{
    public partial class ProcessWindow : Window, IExplorerWindow
    {
        public Grid ContainerFrame { get => Container; }
        public ProcessWindow(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
