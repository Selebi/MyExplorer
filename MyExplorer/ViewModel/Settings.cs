using System.Runtime.Serialization;

namespace MyExplorer.ViewModel
{
    [DataContract]
    internal class Settings : BaseVM
    {
        Services.JsonSerializer<Settings> Serializer;
        bool LoginChanged;
        bool PasswordChanged;

        Settings()
        {
            Serializer = new Services.JsonSerializer<Settings>();
        }
        static Settings _instance;
        public static Settings GetInstance()
        {
            if (_instance == null)
                _instance = new Settings();
            return _instance;
        }

        public RelayCommand Back
        {
            get => new RelayCommand((o) =>
            {
                Services.Navigator.SetPreviousFrame();
            });
        }

        public RelayCommand SaveSettings
        {
            get => new RelayCommand((o) =>
            {
                Save();
            });
        }

        public void Load()
        {
            if (System.IO.File.Exists("Settings.json"))
            {
                var loaded = Serializer.ReadFromFile("Settings.json");
                LogEnabled = loaded.LogEnabled;
                LogFile = loaded.LogFile;
                LocalAdminGroup = loaded.LocalAdminGroup;
                DomainName = loaded.DomainName;
                DoaminAdminGroup = loaded.DoaminAdminGroup;
                DomainLogin = loaded.DomainLogin;
                DomainPassword = loaded.DomainPassword;
                LoginChanged = false;
                PasswordChanged = false;
            }
            else
            {
                Services.FileLogger.GetInstance(LogFile).Write("Файл настроек недоступен", Enums.LogType.Error);
            }
        }

        public void Save()
        {
            if(LoginChanged) DomainLogin = Model.DesSecurity.Encrypt(DomainLogin);
            if(PasswordChanged) DomainPassword = Model.DesSecurity.Encrypt(DomainPassword);
            Serializer.WriteToFile(_instance, "Settings.json");
            LoginChanged = false;
            PasswordChanged = false;
        }

        #region JsonParams

        bool _logEnabled = true;
        string _logFile = "logs.log";
        string _localAdminGroup = "Администраторы";
        string _domainName = "intranet-sintek.net";
        string _domainAdminGroup = "Администраторы домена";
        string _domainLogin = "v.nikitin";
        string _domainPassword = "Fifilafume3108";

        [DataMember]
        public bool LogEnabled
        {
            get => _logEnabled;
            set
            {
                _logEnabled = value;
                OnPropertyChanged();
            }
        }
        [DataMember]
        public string LogFile
        {
            get => _logFile;
            set
            {
                _logFile = value;
                OnPropertyChanged();
            }
        }
        [DataMember]
        public string LocalAdminGroup
        {
            get => _localAdminGroup;
            set
            {
                _localAdminGroup = value;
                OnPropertyChanged();
            }
        }
        [DataMember]
        public string DomainName
        {
            get => _domainName;
            set
            {
                _domainName = value;
                OnPropertyChanged();
            }
        }
        [DataMember]
        public string DoaminAdminGroup
        {
            get => _domainAdminGroup;
            set
            {
                _domainAdminGroup = value;
                OnPropertyChanged();
            }
        }
        [DataMember]
        public string DomainLogin
        {
            get => _domainLogin;
            set
            {
                _domainLogin = value;
                LoginChanged = true;
                OnPropertyChanged();
            }
        }
        [DataMember]
        public string DomainPassword
        {
            get => _domainPassword;
            set
            {
                _domainPassword = value;
                PasswordChanged = true;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
