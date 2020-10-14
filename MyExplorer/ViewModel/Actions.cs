using MyExplorer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.ViewModel
{
    public class Actions: BaseVM
    {
        public Actions()
        {
            Settings.GetInstance().Actions?.ForEach(a =>
            {
                ActionList.Add(new Controls.Action(a));
            });

            Settings.GetInstance().ActionsChanged += la =>
            {
                ActionList.Clear();
                la.ForEach(a =>
                {
                    ActionList.Add(new Controls.Action(a));
                });
            };
        }

        ObservableCollection<Controls.Action> _actionList = new ObservableCollection<Controls.Action>();
        public ObservableCollection<Controls.Action> ActionList
        {
            get => _actionList;
            set
            {
                _actionList = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand Back
        {
            get => new RelayCommand((o) =>
            {
                Navigator.GetInstance(Enums.WindowName.Settings).SetPreviousFrame(Enums.ContainerType.Main);
            });
        }

        public RelayCommand Add
        {
            get => new RelayCommand((o) =>
            {
                Navigator.GetInstance(Enums.WindowName.Settings).ShowModalFrame(Enums.ContainerType.Main, Enums.FrameName.AddAction);
            });
        }
    }
}
