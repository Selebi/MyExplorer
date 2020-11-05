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
                // Первый ли запуск
                bool isFirstLoad = !LoadSettings();
                // Если надо показывать сплеш
                if (settings.ShowSplash == true)
                {
                    splash.Show();
                }
                // Загрузка юзеров
                Model.Users.LoadAll();

                //if (Model.ProcessWorker.IsSExplorerAsAdmin())
                //{
                //    var navigator = Navigator.CreateInstance(Enums.WindowName.Settings);
                //    navigator.ShowWindow();
                //    splash.Close();
                //    return;
                //}

                // Если запуск с пользователем админом
                if (Model.Users.IsAdmin()) // Тест
                {
                    // Если запуск не с правами админа, то перезапуск от админа
                    if (Model.ProcessWorker.RunSExplorerAsAdmin())
                    {
                        // Если первый запуск, сносим настройки
                        if (isFirstLoad)
                            File.Delete("Settings.json");
                        splash.Close();
                        return;
                    }
                    // Запущен ли проводник
                    bool state = Model.ProcessWorker.IsProcessWork("explorer");
                    // Если запущен
                    if (state)
                    {
                        // Запускаем конфигуратор
                        var navigator = Navigator.CreateInstance(Enums.WindowName.Settings);
                        navigator.ShowWindow();
                        StartHotkeyHook();
                        splash.Close();
                    }
                    // Если не запущен
                    else
                    {
                        // Запускаем проводник и вырубаемся
                        Model.ProcessWorker.StartExplorer();
                        splash.Close();
                        return;
                    }
                }
                // Если запуск с пользователем не админом
                else
                {
                    // Если первый запуск
                    if (isFirstLoad)
                    {
                        // Если запуск не с правами админа, то перезапуск от админа
                        if (Model.ProcessWorker.RunSExplorerAsAdmin())
                        {
                            // Сносим настройки
                            File.Delete("Settings.json");
                            splash.Close();
                            return;
                        }
                        // Запускам конфигуратор для настройки при первом запуске
                        var fnavigator = Navigator.CreateInstance(Enums.WindowName.Settings);
                        fnavigator.ShowWindow();
                        StartHotkeyHook();
                        splash.Close();
                    }
                    // Если не первый запуск
                    else
                    {
                        // Если запущено от имени админа
                        if (Model.ProcessWorker.IsSExplorerAsAdmin())
                        {
                            var fnavigator = Navigator.CreateInstance(Enums.WindowName.Settings);
                            fnavigator.ShowWindow();
                            StartHotkeyHook();
                            splash.Close();
                        }
                        // Если запущено не от имени админа
                        else
                        {
                            // Запускаем заданный сценарий
                            var navigator = Navigator.CreateInstance(Enums.WindowName.Process);
                            navigator.ShowWindow();
                            StartHotkeyHook();
                            splash.Close();
                            // Ловим нажатие МастерКея
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SintekExplorerPath - \"{Directory.GetCurrentDirectory()}\". {ex.Message}", "Ошибка запуска", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        void StartHotkeyHook()
        {
            hotkeyLocker = new Services.HotkeyLocker();
            hotkeyLocker.SetHook();
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
