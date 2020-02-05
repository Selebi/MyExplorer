using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.ViewModel
{
    internal class StatusBar : BaseVM
    {
        public StatusBar()
        {
            RegStatus = Model.RegEditor.IsSintekExplorerRegistred();
            Model.RegEditor.RegistredChanged += b => { RegStatus = b; };
        }

        bool _regstatus;
        public bool RegStatus
        {
            get => _regstatus;
            set
            {
                _regstatus = value;
                OnPropertyChanged();
            }
        }
    }
}
