using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class AddAction : UserControl
    {
        public AddAction(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
