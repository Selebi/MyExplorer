using MyExplorer.Enums;
using MyExplorer.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyExplorer
{
    public partial class SettingWindow : Window
    {
        Navigator navigator;

        public SettingWindow(object ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
            Loaded += SettingWindow_Loaded;
            Timers.StartRegExplorerTimer(500);
            Timers.StartWinExplorerTimer(500);
        }

        private void SettingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            navigator = Navigator.GetInstance(WindowName.Settings);
            navigator.AddContainer(ContainerType.Main, Container);
            navigator.SetFrame(FrameName.Main, ContainerType.Main);
        }

        #region Header
        private void Bolder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Navigator.GetInstance(WindowName.Settings).ReleaseFrames(ContainerType.Main);
            Timers.StopTimers();
            this.Close();
        }
        #endregion

        #region Resize
        bool ResizeInProcess = false;
        int ResizePos = 4;

        private void Border_MouseLeftButtonDownResize(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border senderRect)
                if (senderRect != null)
                {
                    ResizeInProcess = true;
                    senderRect.CaptureMouse();
                }
        }

        private void Border_MouseLeftButtonUpResize(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border senderRect)
                if (senderRect != null)
                {
                    ResizeInProcess = false; ;
                    senderRect.ReleaseMouseCapture();
                }
        }

        private void Border_MouseMoveResize(object sender, MouseEventArgs e)
        {
            if (ResizeInProcess && sender is Border senderRect)
            {
                Window mainWindow = senderRect.TemplatedParent as Window;
                double width = e.GetPosition(mainWindow).X;
                double height = e.GetPosition(mainWindow).Y;
                senderRect.CaptureMouse();
                width += ResizePos;
                if (width > 0)
                    mainWindow.Width = width;
                height += ResizePos;
                if (height > 0)
                    mainWindow.Height = height;
            }
        }
        #endregion
    }
}
