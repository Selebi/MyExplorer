using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.ViewModel
{
    public class HotKey : BaseVM
    {
        public HotKey(string text)
        {
            HotkeyText = text;
        }

        public RelayCommand Delete
        {
            get => new RelayCommand((o) =>
            {
                Settings.GetInstance().DelHotkey(HotkeyText);
            });
        }

        public RelayCommand Edit
        {
            get => new RelayCommand((o) =>
            {
                // Может быть потом
            });
        }

        string hotkeyText = "";
        public string HotkeyText
        {
            get => hotkeyText;
            set
            {
                hotkeyText = value;
                OnPropertyChanged();
            }
        }
    }
}
