using System.Windows;

namespace MyExplorer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Splash splash = new Splash();
            splash.Show();

            LoadSettings();
            Model.Users.LoadAll();

            if (Model.Users.IsAdmin())
            {
                new MainWindow(new ViewModel.MainWindow());
                splash.Close();
            }
            else
            {

            }
        }

        void LoadSettings()
        {
            if (!System.IO.File.Exists("Settings.json"))
            {
                ViewModel.Settings.GetInstance().Save();
            }
            else
            {
                ViewModel.Settings.GetInstance().Load();
            }
        }
    }
}
