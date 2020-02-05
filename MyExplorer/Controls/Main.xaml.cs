using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class Main : UserControl
    {
        public Main(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
