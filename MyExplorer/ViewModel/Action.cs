using System.Runtime.Serialization;

namespace MyExplorer.ViewModel
{
    [DataContract]
    public class Action : BaseVM
    {
        string _path;
        string _param;
        uint _delay;
        string _serviceName;

        public RelayCommand Delete
        {
            get => new RelayCommand((o) =>
            {
                Settings.GetInstance().DelAction(this);
            });
        }

        string _pathText;
        public string PathText
        {
            get => _pathText;
            set
            {
                _pathText = value;
                OnPropertyChanged();
            }
        }

        string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        string _error;
        public string Error
        {
            get => _error;
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                if (value != "")
                    PathText = "Запуск программы:";
                OnPropertyChanged();
            }
        }

        [DataMember]
        public string ServiceName
        {
            get => _serviceName;
            set
            {
                _serviceName = value;
                if (value != "")
                    PathText = "Запуск службы:";
                OnPropertyChanged();
            }
        }

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

        [DataMember]
        public uint Delay
        {
            get => _delay;
            set
            {
                _delay = value;
                OnPropertyChanged();
            }
        }
    }
}
