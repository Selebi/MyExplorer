using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.ViewModel
{
    internal class HotKeys : BaseVM
    {
        public RelayCommand Back
        {
            get => new RelayCommand((o) =>
            {
                Services.Navigator.SetPreviousFrame();
            });
        }

        public RelayCommand Add
        {
            get => new RelayCommand((o) =>
            {
                Services.Navigator.SetPreviousFrame();
            });
        }
    }
}
