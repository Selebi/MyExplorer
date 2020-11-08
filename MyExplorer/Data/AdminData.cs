using System.Runtime.Serialization;

namespace MyExplorer.Data
{
    [DataContract]
    public class AdminData : ViewModel.BaseVM
    {
        string _domain = "";
        string _name = "";
        string _password = "";

        public bool DomainChanged { get; set; } = false;
        public bool NameChanged { get; set; } = false;
        public bool PasswordChanged { get; set; } = false;

        [DataMember]
        public string Domain
        {
            get => _domain;
            set
            {
                _domain = value;
                DomainChanged = true;
                OnPropertyChanged();
            }
        }
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NameChanged = true;
                OnPropertyChanged();
            }
        }
        [DataMember]
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                PasswordChanged = true;
                OnPropertyChanged();
            }
        }

        public void Reset()
        {
            DomainChanged = false;
            NameChanged = false;
            PasswordChanged = false;
        }
    }
}
