using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyExplorer.ViewModel
{
    [DataContract]
    public class Settings : BaseVM
    {
        Services.JsonSerializer<Settings> Serializer;
        bool LoginChanged;
        bool PasswordChanged;

        public event Action<List<string>> HotkeysChanged;
        public event Action<List<Action>> ActionsChanged;

        Settings()
        {
            Serializer = new Services.JsonSerializer<Settings>();
            HotKeys = _hotKeys;
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
                Services.Navigator.GetInstance(Enums.WindowName.Settings).SetPreviousFrame(Enums.ContainerType.Main);
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
                HotKeys = loaded.HotKeys;
                Actions = loaded.Actions;
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

        public void AddHotkey(string hotkey)
        {
            HotKeys.Add(hotkey);
            HotKeys = HotKeys;
            Save();
        }

        public void DelHotkey(string hotkey)
        {
            HotKeys.RemoveAll(h => { return h == hotkey; });
            HotKeys = HotKeys;
            Save();
        }

        public void AddAction(Action action)
        {
            Actions.Add(action);
            Actions = Actions;
            Save();
        }

        public void DelAction(Action action)
        {
            Actions.RemoveAll(a => { return a == action; });
            Actions = Actions;
            Save();
        }

        #region HelpingData

        public List<string> HotkeysEnds { get; set; }
        public List<List<string>> HotkeysElements { get; set; }

        #endregion

        #region JsonParams

        bool _logEnabled = true;
        string _logFile = "logs.log";
        string _localAdminGroup = "Администраторы";
        string _domainName = "intranet-sintek.net";
        string _domainAdminGroup = "Администраторы домена";
        string _domainLogin = "login";
        string _domainPassword = "pass";
        List<string> _hotKeys = new List<string>();
        List<Action> _actions = new List<Action>();

        
        [DataMember]
        public List<string> HotKeys 
        {
            get => _hotKeys;
            set
            {
                _hotKeys = value;
                HotkeysEnds = new List<string>();
                HotkeysElements = new List<List<string>>();
                _hotKeys.ForEach(h =>
                {
                    if (h.Contains("+"))
                    {
                        int count = h.Split(new char[] { '+' }).Length;
                        HotkeysEnds.Add(h.Split(new char[] { '+' })[count - 1]);
                        HotkeysElements.Add(new List<string>(h.Split(new char[] { '+' })));
                    }
                    else
                    {
                        HotkeysEnds.Add(h);
                        HotkeysElements.Add(new List<string>(new string[] { h }));
                    }
                });
                HotkeysChanged?.Invoke(_hotKeys);
                OnPropertyChanged();
            }
        }

        [DataMember]
        public List<Action> Actions
        {
            get => _actions;
            set
            {
                _actions = value;
                ActionsChanged?.Invoke(_actions);
                OnPropertyChanged();
            }
        }


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
