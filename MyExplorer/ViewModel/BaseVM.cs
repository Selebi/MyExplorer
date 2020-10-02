using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Threading;

namespace MyExplorer.ViewModel
{
    [DataContract]
    public class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected BaseVM() { }
        public virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Dispatcher MainDispatcher { get; } = Application.Current.MainWindow.Dispatcher;
    }
}