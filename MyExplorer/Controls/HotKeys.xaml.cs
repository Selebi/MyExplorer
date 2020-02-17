using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class HotKeys : UserControl
    {
        public HotKeys(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}