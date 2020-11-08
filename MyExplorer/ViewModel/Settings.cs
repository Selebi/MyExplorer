using MyExplorer.Data;
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
        bool MasterPassChanged;

        public event Action<List<string>> HotkeysChanged;
        public event Action<List<Action>> ActionsChanged;
        public string CurrentFileName { get; private set; }

        Settings()
        {
            CurrentFileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
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
                HotKeys = loaded.HotKeys;
                Actions = loaded.Actions;
                Icons = loaded.Icons;
                MasterKey = loaded.MasterKey;
                MasterPass = loaded.MasterPass;
                ShowSplash = loaded.ShowSplash == null ? false : loaded.ShowSplash;
                DispatcherUser = loaded.DispatcherUser == null ? "" : loaded.DispatcherUser;
                AdminData = loaded.AdminData == null ? new AdminData() : loaded.AdminData;

                LoginChanged = false;
                PasswordChanged = false;
                MasterPassChanged = false;
                AdminData.Reset();
            }
            else
            {
                Services.FileLogger.GetInstance(LogFile).Write("Файл настроек недоступен", Enums.LogType.Error);
            }
        }

        public void Save()
        {
            if (LoginChanged) DomainLogin = Model.DesSecurity.Encrypt(DomainLogin);
            if (PasswordChanged) DomainPassword = Model.DesSecurity.Encrypt(DomainPassword);
            if (MasterPassChanged) MasterPass = Model.Hash.CreateHash(MasterPass);
            if (AdminData.DomainChanged) AdminData.Domain = Model.DesSecurity.Encrypt(AdminData.Domain);
            if (AdminData.NameChanged) AdminData.Name = Model.DesSecurity.Encrypt(AdminData.Name);
            if (AdminData.PasswordChanged) AdminData.Password = Model.DesSecurity.Encrypt(AdminData.Password);
            Serializer.WriteToFile(_instance, "Settings.json");
            LoginChanged = false;
            PasswordChanged = false;
            MasterPassChanged = false;
            AdminData.Reset();
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
        string _masterKey = "A+D+M+N";
        string _masterPass = "65a9a0c386330869187cc08ba9717c42b354b1daaa0ad388c2e735bab9e67a7f";
        List<string> _hotKeys = new List<string>();
        List<Action> _actions = new List<Action>();
        List<Icon> _icons = new List<Icon>();
        bool? _showSplash = false;
        string _dispatcherUser = "";
        AdminData _adminData = new AdminData();

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
        public string MasterKey
        {
            get => _masterKey;
            set
            {
                _masterKey = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public AdminData AdminData
        {
            get => _adminData;
            set
            {
                _adminData = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public string DispatcherUser
        {
            get => _dispatcherUser;
            set
            {
                _dispatcherUser = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public string MasterPass
        {
            get => _masterPass;
            set
            {
                _masterPass = value;
                MasterPassChanged = true;
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
        public bool? ShowSplash
        {
            get => _showSplash;
            set
            {
                _showSplash = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public List<Icon> Icons
        {
            get => _icons;
            set
            {
                _icons = value;
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
