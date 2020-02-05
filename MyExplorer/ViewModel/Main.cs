namespace MyExplorer.ViewModel
{
    internal class Main : BaseVM
    {
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

        public string RegButtonContent
        {
            get
            {
                if (Model.RegEditor.ExplorerRegistred)
                    return "Разрегистрировать";
                else
                    return "Зарегистрировать";
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
                }
                else
                {
                    Model.RegEditor.RegSintekExplorer();
                }
                OnPropertyChanged(RegButtonContent);
            });
        }
    }
}
