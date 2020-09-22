using MyExplorer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.ViewModel
{
    internal class AddAction : BaseVM
    {
        public RelayCommand Cancel
        {
            get => new RelayCommand((o) =>
            {
                Navigator.GetInstance(Navigator.WindowName.Settings).CloseMessage();
            });
        }
    }
}
