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

        string _appPath;
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

        string _param;
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
