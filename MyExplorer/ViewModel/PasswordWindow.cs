using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.ViewModel
{
    public class PasswordWindow : BaseVM
    {
        string _pass = "";
        public string Pass
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
                
            });
        }
    }
}
