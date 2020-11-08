using MyExplorer.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyExplorer
{
    public partial class App : Application
    {
        Services.FileLogger fl;
        Services.HotkeyLocker hotkeyLocker;
        Navigator passNavigator;
        Task TaskUserLoad;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                if (Directory.GetCurrentDirectory().ToLower().Contains("system") || Directory.GetCurrentDirectory().ToLower().Contains("syswow"))
                {
                    Directory.SetCurrentDirectory(File.ReadAllText($"{Environment.SystemDirectory}\\SintekExplorerPath"));
                }
                Splash splash = new Splash();

                // Если запуск выполнен для процесса разрегистрации, запуска проводника и регистрации
                if (e.Args.Length > 0 && e.Args[0] == "reg")
                {
                    LoadSettings();
                    Model.Users.GetSessionUserName();
                    Model.Users.GetCurrentUser();
                    fl = Services.FileLogger.GetInstance(ViewModel.Settings.GetInstance().LogFile);
                    // Запускаем метод разрегистрации / регистрации и убиваемся
                    WaitAndRegSExplorer();
                    splash.Close();
                    return;
                }
                if (e.Args.Length > 0 && e.Args[0] == "runas")
                {
                    if (e.Args.Length > 1 && e.Args[1] == "reg")
                        Model.ProcessWorker.RunSExplorerAsAdmin("reg");
                    else
                        Model.ProcessWorker.RunSExplorerAsAdmin();
                    splash.Close();
                    return;
                }

                if (File.Exists("ready"))
                    File.Delete("ready");

                var settings = ViewModel.Settings.GetInstance();
                // Первый ли запуск и загрузка конфига
                bool isFirstLoad = !LoadSettings();

                fl = Services.FileLogger.GetInstance(ViewModel.Settings.GetInstance().LogFile);

                // Если надо показывать сплеш
                if (settings.ShowSplash == true)
                {
                    splash.Show();
                }
                // Загрузка юзеров
                Model.Users.GetSessionUserName();
                Model.Users.GetCurrentUser();
                TaskUserLoad = Task.Run(() => { Model.Users.LoadAll(); });

                // Если запуск с пользователем не диспетчером
                if (!Model.Users.IsDispatcherUser()) 
                {
                    // Если запущен проводник
                    if (Model.ProcessWorker.IsProcessWork("explorer"))
                    {
                        // Если запущено от админа
                        if (Model.ProcessWorker.IsSExplorerAsAdmin())
                        {
                            // Запускаем конфигуратор
                            File.WriteAllText($"{Environment.SystemDirectory}\\SintekExplorerPath", Directory.GetCurrentDirectory());
                            fl.Write($"SintekExplorerPath установлен - \"{Directory.GetCurrentDirectory()}\"", Enums.LogType.Info);
                            var navigator = Navigator.CreateInstance(Enums.WindowName.Settings);
                            navigator.ShowWindow();
                            StartHotkeyHook();
                            splash.Close();
                        }
                        // Если запущено не от админа
                        else
                        {
                            Model.ProcessWorker.RunProcessAdmin(Directory.GetCurrentDirectory(), $"{Directory.GetCurrentDirectory()}\\{settings.CurrentFileName}", "runas");
                            // Перезапускаемся от админа
                            //Model.ProcessWorker.RunSExplorerAsAdmin();
                            // Если первый запуск, сносим настройки
                            if (isFirstLoad)
                                File.Delete("Settings.json");
                            splash.Close();
                            return;
                        }
                    }
                    // Если проводник не запущен
                    else
                    {
                        // Если запущено от админа
                        if (Model.ProcessWorker.IsSExplorerAsAdmin())
                        {
                            // Запускаем конфигуратор
                            var navigator = Navigator.CreateInstance(Enums.WindowName.Settings);
                            navigator.ShowWindow();
                            StartHotkeyHook();
                            splash.Close();
                        }
                        // Если запущено не от админа
                        else
                        {
                            Model.ProcessWorker.RunProcessAdmin(Directory.GetCurrentDirectory(), $"{Directory.GetCurrentDirectory()}\\{settings.CurrentFileName}", "runas reg");
                            // Запускаем вторую копию от админа для управления реестром
                            //Model.ProcessWorker.RunSExplorerAsAdmin("reg");
                            // Запускаем метод запуска проводника
                            WaitAndStartExplorer();
                            // Вырубаемся
                            splash.Close();
                            return;
                        }
                    }
                }
                // Если запуск с пользователем диспетчером
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
                MessageBox.Show($"SintekExplorerPath - \"{Directory.GetCurrentDirectory()}\". {ex}", "Ошибка запуска", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        void WaitAndStartExplorer()
        {
            fl.Write("Start Reg Waiting...", Enums.LogType.Info);
            // Ждём пока не будет разреган наш експлорер
            while (!File.Exists("ready"))
            {
                Thread.Sleep(500);
            }
            fl.Write("Start WinExplorer", Enums.LogType.Info);
            // Запускаем проводник от локальной учётки
            Model.ProcessWorker.StartExplorer(true);
        }

        void WaitAndRegSExplorer()
        {
            fl.Write("Reg WinExplorer", Enums.LogType.Info);
            // Регаем виндовый проводник
            Model.RegEditor.RegWindowsExplorer();
            fl.Write("Send Ready", Enums.LogType.Info);
            File.Create("ready").Close();
            // Ждём пока виндовый проводник не запустится копией SintekExplorer-а из-под локальной учётки
            fl.Write("Start WinExplorer Waiting...", Enums.LogType.Info);
            while (!Model.ProcessWorker.IsProcessWork("explorer"))
            {
                Thread.Sleep(500);
            }
            // Ждём одупления виндового Эксплорера
            Thread.Sleep(2000);
            fl.Write("Reg SintekExplorer", Enums.LogType.Info);
            // Обратно регаем свой експлорер
            Model.RegEditor.RegSintekExplorer();
            File.Delete("ready");
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
