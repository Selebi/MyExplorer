using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyExplorer
{
    public partial class PasswordWindow : Window
    {
        public PasswordWindow(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;

            Task.Run(() =>
            {
                for (int i = 200; i > 0; i--)
                {
                    this.Dispatcher.Invoke(() => { Langv.Text = System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.ThreeLetterWindowsLanguageName; });
                    Thread.Sleep(100);
                }
            });

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
