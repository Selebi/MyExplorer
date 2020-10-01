using System.Threading;
using System.Windows;

namespace MyExplorer
{
    public partial class App : Application
    {
        Services.HotkeyLocker hotkeyLocker;
        Window passWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            Splash splash = new Splash();
            splash.Show();

            LoadSettings();
            Model.Users.LoadAll();

            if (!Model.Users.IsAdmin()) // Тест
            {
                new SettingWindow(new ViewModel.SettingWindow());
                splash.Close();
                Services.Timers.StartRegExplorerTimer(500);
                Services.Timers.StartWinExplorerTimer(500);
            }
            else
            {
                new ProcessWindow(new ViewModel.ProcessWindow());
                splash.Close();
                Model.HotkeyProcessor.MasterKeyDetected += () =>
                {
                    if(passWindow == null || !passWindow.IsLoaded)
                        passWindow = new PasswordWindow(new ViewModel.PasswordWindow());
                };
            }
            hotkeyLocker = new Services.HotkeyLocker();
            hotkeyLocker.SetHook();
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
