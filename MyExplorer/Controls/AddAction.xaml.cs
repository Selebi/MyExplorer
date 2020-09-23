using System.Windows.Controls;

namespace MyExplorer.Controls
{
    public partial class AddAction : UserControl
    {
        public AddAction(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
            Loaded += (o, e) => {
                ServiceListBox.SelectedItem = null;
                PathTextBox.Text = "";
                ParamTextBox.Text = "";
                service.Checked += (o1, e1) => {
                    PathTextBox.Text = "";
                    ParamTextBox.Text = "";
                };
                program.Checked += (o1, e1) =>
                {
                    ServiceListBox.SelectedItem = null;
                };
            };
        }
    }
}
