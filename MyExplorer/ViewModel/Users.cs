using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.ViewModel
{
    public class Users : BaseVM
    {
        public Users()
        {
            UserList = new ObservableCollection<Data.User>();
            InitData();
        }

        void InitData()
        {
            UserList.Clear();
            Model.Users.UsersLocalGroup.ForEach(u =>
            {
                UserList.Add(u);
            });
            Model.Users.UsersMembersDomainGroup.ForEach(u =>
            {
                UserList.Add(u);
            });
            OnPropertyChanged("UserList");
            User = $"{Model.Users.SessionUser.Domain}/{Model.Users.SessionUser.Name}";
        }

        public RelayCommand Back
        {
            get => new RelayCommand((o) =>
            {
                Services.Navigator.GetInstance(Enums.WindowName.Settings).SetPreviousFrame(Enums.ContainerType.Main);
            });
        }

        public RelayCommand Update
        {
            get => new RelayCommand((o) =>
            {
                Model.Users.LoadAll();
                InitData();
            });
        }

        public ObservableCollection<Data.User> UserList { get; set; }

        string _user;
        public string User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }
    }
}
