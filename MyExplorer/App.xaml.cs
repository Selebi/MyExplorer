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
                    if (RunAdmin())
                    {
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

        bool RunAdmin()
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);

            if (hasAdministrativeRight == false)
            {
                ProcessStartInfo processInfo = new ProcessStartInfo(); //создаем новый процесс
                processInfo.Verb = "runas"; 
                processInfo.FileName = "Sintek Explorer.exe"; 
                try
                {
                    Process.Start(processInfo); 
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"SintekExplorerPath - \"{Directory.GetCurrentDirectory()}\". {ex.Message}", "Ошибка запроса привелегий", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }
    }
}
