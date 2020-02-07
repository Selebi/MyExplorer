using System.Windows;

namespace MyExplorer
{
    public partial class App : Application
    {
        Services.HotkeyLocker hotkeyLocker;

        protected override void OnStartup(StartupEventArgs e)
        {
            Splash splash = new Splash();
            splash.Show();

            LoadSettings();
            Model.Users.LoadAll();

            if (Model.Users.IsAdmin())
            {
                new SettingWindow(new ViewModel.SettingWindow());
                splash.Close();
            }
            else
            {
                new ProcessWindow(new ViewModel.ProcessWindow());
                splash.Close();
            }
            hotkeyLocker = new Services.HotkeyLocker();
            hotkeyLocker.SetHook();

            Services.Timers.StartRegExplorerTimer(500);
            Services.Timers.StartWinExplorerTimer(500);
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
