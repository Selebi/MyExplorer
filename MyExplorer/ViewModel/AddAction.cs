using MyExplorer.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;

namespace MyExplorer.ViewModel
{
    public class AddAction : BaseVM
    {
        List<ServiceController> ServicesCache;
        public AddAction()
        {
            ServicesCache = Model.ServiceWorker.GetAllServices();
            ServiceList = new ObservableCollection<ServiceController>(ServicesCache);
        }

        public string Search
        {
            set
            {
                ServiceList.Clear();
                if (value == "")
                {
                    ServicesCache.ForEach(s => { ServiceList.Add(s); });
                }
                else
                {
                    ServicesCache.FindAll(s => { return s.ServiceName.ToLower().Contains(value.ToLower()) || s.DisplayName.ToLower().Contains(value.ToLower()); }).ForEach(sr =>
                    {
                        ServiceList.Add(sr);
                    });
                }
            }
        }

        string _serviceName = "";
        public string ServiceName
        {
            get => _serviceName;
            set
            {
                _serviceName = value;
                OnPropertyChanged();
            }
        }

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

        public ObservableCollection<ServiceController> ServiceList { get; set; } = new ObservableCollection<ServiceController>();

        ServiceController _selectedService;
        public ServiceController SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                if (value != null)
                    ServiceName = value.ServiceName;
                else
                    ServiceName = "";
                OnPropertyChanged();
            }
        }

        public RelayCommand Cancel
        {
            get => new RelayCommand((o) =>
            {
                Navigator.GetInstance(Enums.WindowName.Settings).CloseMessage(Enums.ContainerType.Main);
            });
        }

        public RelayCommand Add
        {
            get => new RelayCommand((o) =>
            {

                var action = new Action() { Path = _path, Param = _param, Delay = _delay, ServiceName = _serviceName };
                Settings.GetInstance().AddAction(action);
                Navigator.GetInstance(Enums.WindowName.Settings).CloseMessage(Enums.ContainerType.Main);
            },
                o => { return _path != "" || _serviceName != ""; });
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
