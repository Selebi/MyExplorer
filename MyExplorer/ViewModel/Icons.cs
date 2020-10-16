using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.ViewModel
{
    public class Icons : BaseVM
    {
        public ObservableCollection<Controls.Icon> IconList { get; set; } = new ObservableCollection<Controls.Icon>();

        public Icons()
        {
            IconList.Add(new Controls.Icon(new Icon() { Text = "Типа первая иконка с длинным именем", ImagePath = "D:\\img\\Камера_реактора_(IndustrialCraft_2).png" }));
            IconList.Add(new Controls.Icon(new Icon() { Text = "Типа вторая иконка", ImagePath = "D:\\img\\Корпус_компьютера3_(OpenComputers).png" }));
            IconList.Add(new Controls.Icon(new Icon() { Text = "Третья", ImagePath = "D:\\img\\Server_(OpenComputers).gif" }));
            IconList.Add(new Controls.Icon(new Icon() { Text = "Типа первая иконка с длинным именем", ImagePath = "D:\\img\\Камера_реактора_(IndustrialCraft_2).png" }));
            IconList.Add(new Controls.Icon(new Icon() { Text = "Типа вторая иконка", ImagePath = "D:\\img\\Корпус_компьютера3_(OpenComputers).png" }));
            IconList.Add(new Controls.Icon(new Icon() { Text = "Третья", ImagePath = "D:\\img\\Server_(OpenComputers).gif" }));
            IconList.Add(new Controls.Icon(new Icon() { Text = "Типа первая иконка с длинным именем", ImagePath = "D:\\img\\Камера_реактора_(IndustrialCraft_2).png" }));
            IconList.Add(new Controls.Icon(new Icon() { Text = "Типа вторая иконка", ImagePath = "D:\\img\\Корпус_компьютера3_(OpenComputers).png" }));
            IconList.Add(new Controls.Icon(new Icon() { Text = "Третья", ImagePath = "D:\\img\\Server_(OpenComputers).gif" }));
        }
    }
}
