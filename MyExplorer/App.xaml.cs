using MyExplorer.Services;
using System;
using System.IO;
using System.Windows;

namespace MyExplorer
{
    public partial class App : Application
    {
        Services.HotkeyLocker hotkeyLocker;
        Navigator passNavigator;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Splash splash = new Splash();
                splash.Show();
                if (Directory.GetCurrentDirectory().ToLower().Contains("system32") || Directory.GetCurrentDirectory().ToLower().Contains("syswow64"))
                {
                    Directory.SetCurrentDirectory(File.ReadAllText($"{Environment.SystemDirectory}\\SintekExplorerPath"));
                }
                LoadSettings();
                Model.Users.LoadAll();

                if (Model.Users.IsAdmin()) // Тест
                {
                    var navigator = Navigator.CreateInstance(Enums.WindowName.Settings);
                    navigator.ShowWindow();
                    splash.Close();
                }
                else
                {
                    var navigator = Navigator.CreateInstance(Enums.WindowName.Process);
                    navigator.ShowWindow();
                    splash.Close();

                    Model.HotkeyProcessor.MasterKeyDetected += () =>
                    {
                        if (passNavigator == null || !passNavigator.IsLoaded())
                        {
                            passNavigator = Navigator.CreateInstance(Enums.WindowName.Password);
                            passNavigator.ShowWindow();
                        }
                    };
                }
                hotkeyLocker = new Services.HotkeyLocker();
                hotkeyLocker.SetHook();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SintekExplorerPath - \"{Directory.GetCurrentDirectory()}\". {ex.Message}", "Ошибка запуска", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
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
