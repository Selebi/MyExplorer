using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class Process : UserControl
    {
        public Process(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
