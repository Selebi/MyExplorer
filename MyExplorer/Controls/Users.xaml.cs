using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class Users : UserControl
    {
        public Users(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
