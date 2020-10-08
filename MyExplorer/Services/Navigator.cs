using MyExplorer.Data;
using MyExplorer.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyExplorer.Services
{
    public class Navigator
    {
        WindowName currentWindow = WindowName.Null;
        Window window;

        Dictionary<ContainerType, Container> containers = new Dictionary<ContainerType, Container>();

        static List<Navigator> navigators = new List<Navigator>();

        Navigator() { }

        public static Navigator GetInstance(WindowName window)
        {
            if (navigators.Any(n => { return n.currentWindow == window; }))
                return navigators.Find(n => { return n.currentWindow == window; });
            else
                return null;
        }

        public static void CreateInstance(WindowName windowName)
        {
            if (navigators.Any(n => { return n.currentWindow == windowName; }))
            {
                navigators.RemoveAll(n => { return n.currentWindow == windowName; });
            }
            if (Windows.TryGetValue(windowName, out object window))
            {
                Navigator navigator = new Navigator();
                navigators.Add(navigator);
                navigator.currentWindow = windowName;
                navigator.window = (Window)window;
                navigator.window.Show();
            }
        }

        #region Frames

        static Dictionary<FrameName, object> Frames = new Dictionary<FrameName, object>()
        {
            {FrameName.Main, new Controls.Main(new ViewModel.Main())},
            {FrameName.Settings, new Controls.Settings(ViewModel.Settings.GetInstance())},
            {FrameName.Users, new Controls.Users(new ViewModel.Users())},
            {FrameName.Actions, new Controls.Actions(new ViewModel.Actions())},
            {FrameName.Journal, new Controls.Journal(new ViewModel.Journal())},
            {FrameName.Hotkeys, new Controls.HotKeys(new ViewModel.HotKeys())},
            {FrameName.AddHotKey, new Controls.AddHotKey(new ViewModel.AddHotKey())},
            {FrameName.AddAction, new Controls.AddAction(new ViewModel.AddAction())},
            {FrameName.Process, new Controls.Process(ViewModel.Process.GetInstance())},
            {FrameName.StatusBar, new Controls.StatusBar(new ViewModel.StatusBar())}
        };

        static Dictionary<WindowName, object> Windows = new Dictionary<WindowName, object>()
        {
            { WindowName.Settings, new SettingWindow(new ViewModel.SettingWindow()) },
            { WindowName.Process, new ProcessWindow(new ViewModel.ProcessWindow()) },
            { WindowName.Icons, new IconsWindow(new ViewModel.IconsWindow()) },
            { WindowName.Password, new PasswordWindow(new ViewModel.PasswordWindow()) }
        };

        #endregion

        public void AddContainer(ContainerType containerType, Grid containerUI)
        {
            if (!containers.ContainsKey(containerType))
            {
                Container container = new Container();
                container.ContainerGrid = containerUI;
                containers.Add(containerType, container);
            }
        }

        public void ReleaseFrames(ContainerType containerType)
        {
            if (containers.TryGetValue(containerType, out Container container))
            {
                container.ContainerGrid.Children.Clear();
            }
        }

        public void SetFrame(FrameName frameName, ContainerType containerType)
        {
            if (containers.TryGetValue(containerType, out Container container))
            {
                if (Frames.TryGetValue(frameName, out object value))
                {
                    container.ContainerGrid.Children.Clear();
                    container.ContainerGrid.Children.Add((UIElement)value);
                    container.PreviousUI.Push(container.CurrentUI);
                    container.CurrentUI = frameName;
                }
            }
        }

        public void SetPreviousFrame(ContainerType containerType)
        {
            if (containers.TryGetValue(containerType, out Container container))
            {
                var frame = container.PreviousUI.Pop(); // Отсылка к warframe
                if (Frames.TryGetValue(frame, out object value))
                {
                    container.ContainerGrid.Children.Clear();
                    container.ContainerGrid.Children.Add((UIElement)value);
                    container.CurrentUI = frame;
                }
            }
        }

        public void ShowMessage(ContainerType containerType, FrameName frameMessage)
        {
            if (containers.TryGetValue(containerType, out Container container))
            {
                if (Frames.TryGetValue(frameMessage, out object value))
                {
                    container.ContainerGrid.Children[container.ContainerGrid.Children.Count - 1].IsEnabled = false;
                    container.ContainerGrid.Children.Add((UIElement)value);
                }
            }
        }

        public void CloseMessage(ContainerType containerType)
        {
            if (containers.TryGetValue(containerType, out Container container))
            {
                if (container.ContainerGrid.Children.Count >= 2)
                {
                    container.ContainerGrid.Children.RemoveAt(container.ContainerGrid.Children.Count - 1);
                    container.ContainerGrid.Children[container.ContainerGrid.Children.Count - 1].IsEnabled = true;
                }
            }
        }
    }
}
