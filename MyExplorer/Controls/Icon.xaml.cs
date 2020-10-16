using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class Icon : UserControl
    {
        public Icon(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
