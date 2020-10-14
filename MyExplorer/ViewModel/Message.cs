using MyExplorer.Enums;
using MyExplorer.Services;

namespace MyExplorer.ViewModel
{
    public class Message : BaseVM
    {
        public Message(string header, string message, MessageType type)
        {
            HeaderText = header;
            MessageText = message;
            Type = type;
        }

        string _header;
        public string HeaderText
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged();
            }
        }

        string _message;
        public string MessageText
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        MessageType _type;
        public MessageType Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand Close
        {
            get => new RelayCommand((o) =>
            {
                Navigator.ClodeLastModal();
            });
        }
    }
}
