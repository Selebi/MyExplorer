using MyExplorer.Services;
using System.Runtime.Serialization;

namespace MyExplorer.ViewModel
{
    [DataContract]
    public class Icon : BaseVM
    {
        public string ToolTip
        {
            get => Text;
        }

        public RelayCommand Start
        {
            get => new RelayCommand((o) =>
            {
                string error = Model.ProcessWorker.StartProcess(AppPath, Param);
                if (error != "")
                    Navigator.GetInstance(Enums.WindowName.Icons).ShowModalMessage(Enums.ContainerType.Main, Enums.MessageType.Error, "Ошибка запуска", error);
            });
        }

        string _imagePath;
        [DataMember]
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        string _text;
        [DataMember]
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        string _appPath = "";
        [DataMember]
        public string AppPath
        {
            get => _appPath;
            set
            {
                _appPath = value;
                OnPropertyChanged();
            }
        }

        string _param = "";
        [DataMember]
        public string Param
        {
            get => _param;
            set
            {
                _param = value;
                OnPropertyChanged();
            }
        }
    }
}
