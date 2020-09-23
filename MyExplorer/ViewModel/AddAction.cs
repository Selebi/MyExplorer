using MyExplorer.Services;
using System.Linq;
using System.Windows.Forms;

namespace MyExplorer.ViewModel
{
    public class AddAction : BaseVM
    {
        string _path = "";
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        string _param = "";
        public string Param
        {
            get => _param;
            set
            {
                _param = value;
                OnPropertyChanged();
            }
        }

        uint _delay = 500;
        public uint Delay
        {
            get => _delay;
            set
            {
                _delay = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand Cancel
        {
            get => new RelayCommand((o) =>
            {
                Navigator.GetInstance(Navigator.WindowName.Settings).CloseMessage();
            });
        }

        public RelayCommand Add
        {
            get => new RelayCommand((o) =>
            {
                var action = new Action() { Path = _path, Param = _param, Delay = _delay };
                Settings.GetInstance().AddAction(action);
                Navigator.GetInstance(Navigator.WindowName.Settings).CloseMessage();
            },
                o => { return _path != ""; });
        }

        public RelayCommand Browse
        {
            get => new RelayCommand((o) =>
            {
                var ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Path = ofd.FileName;
                }
            });
        }
    }
}
