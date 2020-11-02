using MyExplorer.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
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
                if (Directory.GetCurrentDirectory().ToLower().Contains("system32") || Directory.GetCurrentDirectory().ToLower().Contains("syswow64"))
                {
                    Directory.SetCurrentDirectory(File.ReadAllText($"{Environment.SystemDirectory}\\SintekExplorerPath"));
                }
                Splash splash = new Splash();

                var settings = ViewModel.Settings.GetInstance();

                bool isFirstLoad = !LoadSettings();

                if (settings.ShowSplash == true)
                {
                    splash.Show();
                }
                Model.Users.LoadAll();

                if (Model.ProcessWorker.IsSExplorerAsAdmin())
                {
                    var navigator = Navigator.CreateInstance(Enums.WindowName.Settings);
                    navigator.ShowWindow();
                    splash.Close();
                    return;
                }

                if (Model.Users.IsAdmin()) // Тест
                {
                    if (Model.ProcessWorker.RunSExplorerAsAdmin())
                    {
                        if (isFirstLoad)
                            File.Delete("Settings.json");
                        splash.Close();
                        return;
                    }
                    bool state = Model.ProcessWorker.IsProcessWork("explorer");
                    if (state)
                    {
                        var navigator = Navigator.CreateInstance(Enums.WindowName.Settings);
                        navigator.ShowWindow();
                        splash.Close();
                    }
                    else
                    {
                        Model.ProcessWorker.StartExplorer();
                        splash.Close();
                        return;
                    }
                }
                else
                {
                    if (isFirstLoad)
                    {
                        if (Model.ProcessWorker.RunSExplorerAsAdmin())
                        {
                            File.Delete("Settings.json");
                            splash.Close();
                            return;
                        }
                        var fnavigator = Navigator.CreateInstance(Enums.WindowName.Settings);
                        fnavigator.ShowWindow();
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

        bool LoadSettings()
        {
            if (!System.IO.File.Exists("Settings.json"))
            {
                ViewModel.Settings.GetInstance().Save();
                return false;
            }
            else
            {
                ViewModel.Settings.GetInstance().Load();
                return true;
            }
        }
    }
}
