namespace MyExplorer.ViewModel
{
    internal class Main : BaseVM
    {
        public Main()
        {
            if (Model.RegEditor.ExplorerRegistred)
                _regButtonContent = "Разрегистрировать";
            else
                _regButtonContent = "Зарегистрировать";
        }

        string _helloText = "Место для вашей рекламы!";
        public string HelloText
        {
            get => _helloText;
            set
            {
                _helloText = value;
                OnPropertyChanged();
            }
        }

        string _regButtonContent;
        public string RegButtonContent
        {
            get => _regButtonContent;
            set
            {
                _regButtonContent = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand OpenSettings
        {
            get => new RelayCommand(o =>
            {
                Services.Navigator.SetFrame(Services.Navigator.FrameName.Settings);
            });
        }

        public RelayCommand OpenUsers
        { 
            get => new RelayCommand(o =>
            {
                Services.Navigator.SetFrame(Services.Navigator.FrameName.Users);
            });
        }

        public RelayCommand OpenJournal
        {
            get => new RelayCommand(o =>
            {
                Services.Navigator.SetFrame(Services.Navigator.FrameName.Journal);
            });
        }

        public RelayCommand RegExplorer
        {
            get => new RelayCommand(o =>
            {
                if (Model.RegEditor.ExplorerRegistred)
                {
                    Model.RegEditor.RegWindowsExplorer();
                    RegButtonContent = "Зарегистрировать";
                }
                else
                {
                    Model.RegEditor.RegSintekExplorer();
                    RegButtonContent = "Разрегистрировать";
                }
            });
        }
    }
}
