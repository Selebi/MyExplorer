using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class AddHotKey : UserControl
    {
        public AddHotKey(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
