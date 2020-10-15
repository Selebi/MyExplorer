using MyExplorer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyExplorer.ViewModel
{
    public class PasswordWindow : BaseVM
    {
        string _pass = "";
        public string Password
        {
            get => _pass;
            set
            {
                _pass = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand Enter
        {
            get => new RelayCommand((o) =>
            {
                if(o is PasswordBox pb)
                {
                    if(Model.Hash.CreateHash(pb.Password) == Settings.GetInstance().MasterPass)
                    {
                        var settingNav = Navigator.CreateInstance(Enums.WindowName.Settings);
                        settingNav.ShowWindow();
                    }
                    Navigator.GetInstance(Enums.WindowName.Password).CloseWindow();
                }
            });
        }
    }
}
