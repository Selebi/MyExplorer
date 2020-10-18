using System.Collections.ObjectModel;

namespace MyExplorer.ViewModel
{
    public class Icons : BaseVM
    {
        public ObservableCollection<Controls.Icon> IconList { get; set; } = new ObservableCollection<Controls.Icon>();

        public Icons()
        {
            Settings.GetInstance().Icons.ForEach(i =>
            {
                IconList.Add(new Controls.Icon(i));
            });
        }
    }
}
