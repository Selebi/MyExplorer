using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class Action : UserControl
    {
        public Action(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
