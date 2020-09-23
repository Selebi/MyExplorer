using System.Runtime.Serialization;

namespace MyExplorer.ViewModel
{
    [DataContract]
    public class Action : BaseVM
    {
        string _path;
        string _param;
        uint _delay;

        [DataMember]
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
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
