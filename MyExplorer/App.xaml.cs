using MyExplorer.Services;
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

            if (Model.Users.IsAdmin()) // Тест
            {
                var navigator = Navigator.CreateInstance(Enums.WindowName.Settings);
                navigator.ShowWindow(Enums.WindowName.Settings);
                splash.Close();
            }
            else
            {
                var navigator = Navigator.CreateInstance(Enums.WindowName.Process);
                navigator.ShowWindow(Enums.WindowName.Process);
                splash.Close();
                var PassVM = new ViewModel.PasswordWindow();
                PassVM.Pass += () => 
                { 
                    passWindow.Close();
                    Navigator.CreateInstance(Enums.WindowName.Settings);
                };
                Model.HotkeyProcessor.MasterKeyDetected += () =>
                {
                    if(passWindow == null || !passWindow.IsLoaded)
                        passWindow = new PasswordWindow(PassVM);
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
