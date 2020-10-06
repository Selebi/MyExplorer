﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyExplorer.ViewModel
{
    public class PasswordWindow : BaseVM
    {
        public event System.Action Pass;

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
                    if(pb.Password == "Adrenalin123")
                    {
                        Pass?.Invoke();
                    }
                }
            });
        }
    }
}
