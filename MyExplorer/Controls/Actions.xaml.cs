using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class Actions : UserControl
    {
        public Actions(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
