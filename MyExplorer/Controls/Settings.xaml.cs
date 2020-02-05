using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class Settings : UserControl
    {
        public Settings(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
