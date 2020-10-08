using System.Windows;

namespace MyExplorer
{
    public partial class ProcessWindow : Window
    {
        public ProcessWindow(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
