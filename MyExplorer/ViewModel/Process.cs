using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyExplorer.ViewModel
{
    public class Process : BaseVM
    {
        public ObservableCollection<Action> Actions { get; set; }

        Process() { }
        static Process _instance;
        public static Process GetInstance()
        {
            if (_instance == null)
                _instance = new Process();
            return _instance;
        }

        public async Task Start()
        {
            await Task.Run(() =>
            {
                foreach(var a in Actions)
                {
                    MainDispatcher.Invoke(() => { a.Status = "Запуск..."; });
                    if (a.Path != null && a.Path != "")
                    {
                        Model.ProcessWorker.StartProcess(a.Path);
                        for (int i = 0; i < 100; i++)
                        {
                            Thread.Sleep(100);
                            if (Model.ProcessWorker.IsStartedProcessWork(a.Path))
                                break;
                        }
                        a.Error = "Ебать, Ошибка!!!";
                    }
                    MainDispatcher.Invoke(() => { a.Status = "Выполнено"; });
                    Thread.Sleep((int)a.Delay);
                }
            });
        }
    }
}
