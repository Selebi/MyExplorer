using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class Icons : UserControl
    {
        public Icons(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
