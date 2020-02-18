using MyExplorer.Services;

namespace MyExplorer.ViewModel
{
    internal class HotKeys : BaseVM
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
                Navigator.GetInstance(Navigator.WindowName.Settings).ShowMessage(Navigator.FrameName.AddHotKey);
            });
        }
    }
}
