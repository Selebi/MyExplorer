using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class HotKey : UserControl
    {
        public HotKey(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
