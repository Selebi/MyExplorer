using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyExplorer
{
    public partial class PasswordWindow : Window
    {
        public PasswordWindow(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
            Show();

            pb.Focus();
            Task.Run(() =>
            {
                for (int i = 20; i > 0; i--)
                {
                    this.Dispatcher.Invoke(() => { timer.Text = i.ToString(); });
                    Thread.Sleep(1000);
                }
                this.Dispatcher.Invoke(() => { this.Close(); });
            });
        }
    }
}
