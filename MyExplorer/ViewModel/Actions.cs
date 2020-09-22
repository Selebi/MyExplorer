using MyExplorer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.ViewModel
{
    internal class Actions: BaseVM
    {
        public RelayCommand Back
        {
            get => new RelayCommand((o) =>
            {
                Navigator.GetInstance(Navigator.WindowName.Settings).SetPreviousFrame();
            });
        }

        public RelayCommand Add
        {
            get => new RelayCommand((o) =>
            {
                Navigator.GetInstance(Navigator.WindowName.Settings).ShowMessage(Navigator.FrameName.AddAction);
            });
        }
    }
}
