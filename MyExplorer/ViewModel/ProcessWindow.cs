using MyExplorer.Enums;
using MyExplorer.Services;

namespace MyExplorer.ViewModel
{
    public class ProcessWindow : BaseVM
    {
        Settings settings;
        public ProcessWindow()
        {
            settings = Settings.GetInstance();

            Process.GetInstance().Done += ProcessWindow_Done;
            Process.GetInstance().Start(settings.Actions);
        }

        private void ProcessWindow_Done()
        {
            Navigator.GetInstance(WindowName.Process).HideWindow();
        }
    }
}
