using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class StatusBar : UserControl
    {
        public StatusBar(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
