using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class Journal : UserControl
    {
        public Journal(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
