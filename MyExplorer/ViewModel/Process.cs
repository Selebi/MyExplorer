using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace MyExplorer.ViewModel
{
    public class Process : BaseVM
    {
        Services.FileLogger fl = Services.FileLogger.GetInstance(ViewModel.Settings.GetInstance().LogFile);
        public ObservableCollection<Action> ViewActions { get; set; } = new ObservableCollection<Action>();
        public event System.Action Done;

        Process() { }
        static Process _instance;
        public static Process GetInstance()
        {
            if (_instance == null)
                _instance = new Process();
            return _instance;
        }

        public async Task Start(List<Action> Actions)
        {
            await Task.Run(() =>
            {
                string error;
                foreach (var a in Actions)
                {
                    try
                    {
                        MainDispatcher.Invoke(() => { ViewActions.Insert(0, a); });
                        MainDispatcher.Invoke(() => { a.Status = "Запуск..."; });
                        if (a.Path != null && a.Path != "")
                        {
                            error = Model.ProcessWorker.StartProcess(a.Path, a.Param);
                            MainDispatcher.Invoke(() => { a.Error = error; });
                            for (int i = 0; i < 100; i++)
                            {
                                Thread.Sleep(100);
                                if (Model.ProcessWorker.IsStartedProcessWork(a.Path))
                                    break;
                            }
                        }
                        else
                        {
                            //Обработка запуска процессов
                        }
                        MainDispatcher.Invoke(() => { a.Status = "Выполнено"; });
                        fl.Write($"Запущено {(a.Path != "" ? a.Path : a.ServiceName)}", Enums.LogType.Info);
                    }
                    catch (Exception ex)
                    {
                        MainDispatcher.Invoke(() => { a.Status = "Не выполнено"; });
                        MainDispatcher.Invoke(() => { a.Error = $"Ошибка: {ex.Message}"; });
                        fl.Write($"Ошибка при запуске {a.Path} - {ex.Message}", Enums.LogType.Info);
                    }
                    Thread.Sleep((int)a.Delay);
                }
                Thread.Sleep(1000);
            });
            Done?.Invoke();
        }
    }
}
