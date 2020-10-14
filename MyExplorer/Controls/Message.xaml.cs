using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class Message : UserControl
    {
        public Message(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
