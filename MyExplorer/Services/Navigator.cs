using MyExplorer.Controls;
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

        static Stack<(Navigator navigator, ContainerType container)> OpenedMessages = new Stack<(Navigator navigator, ContainerType container)>();

        Navigator() { }

        public static Navigator GetInstance(WindowName window)
        {
            if (navigators.Any(n => { return n.currentWindow == window; }))
                return navigators.Find(n => { return n.currentWindow == window; });
            else
                return null;
        }

        public static Navigator CreateInstance(WindowName windowName)
        {
            if (navigators.Any(n => { return n.currentWindow == windowName; }))
            {
                navigators.RemoveAll(n => { return n.currentWindow == windowName; });
            }
            Navigator navigator = new Navigator();
            navigators.Add(navigator);
            navigator.currentWindow = windowName;
            if (Windows.TryGetValue(windowName, out object window))
            {
                navigator.window = (Window)window;
            }
            return navigator;
        }

        public void ShowWindow()
        {
            if (Windows.TryGetValue(currentWindow, out object _window))
            {
                ((Window)_window).Show();
            }
            else
            {
                object w = getWindow(currentWindow);
                if(w != null)
                {
                    Windows.Add(currentWindow, w);
                    window = (Window)w;
                    window.Show();
                }
            }
        }

        public void CloseWindow()
        {
            Windows.Remove(currentWindow);
            window.Close();
        }

        public void HideWindow()
        {
            window.Visibility = Visibility.Collapsed;
        }

        public bool IsLoaded()
        {
            return window.IsLoaded;
        }

        #region Frames

        static Dictionary<FrameName, object> Frames = new Dictionary<FrameName, object>();

        static Dictionary<WindowName, object> Windows = new Dictionary<WindowName, object>();

        static object getFrame(FrameName name)
        {
            switch (name)
            {
                case FrameName.Actions: return new Actions(new ViewModel.Actions());
                case FrameName.AddAction: return new AddAction(new ViewModel.AddAction());
                case FrameName.AddHotKey: return new AddHotKey(new ViewModel.AddHotKey());
                case FrameName.Hotkeys: return new HotKeys(new ViewModel.HotKeys());
                case FrameName.Journal: return new Journal(new ViewModel.Journal());
                case FrameName.Main: return new Main(new ViewModel.Main());
                case FrameName.Settings: return new Settings(ViewModel.Settings.GetInstance());
                case FrameName.Process: return new Process(ViewModel.Process.GetInstance());
                case FrameName.StatusBar: return new StatusBar(new ViewModel.StatusBar());
                case FrameName.Users: return new Users(new ViewModel.Users());
            }
            return null;
        }

        static object getWindow(WindowName name)
        {
            switch (name)
            {
                case WindowName.Settings: return new SettingWindow(new ViewModel.SettingWindow());
                case WindowName.Process: return new ProcessWindow(new ViewModel.ProcessWindow());
                case WindowName.Icons: return new IconsWindow(new ViewModel.IconsWindow());
                case WindowName.Password: return new PasswordWindow(new ViewModel.PasswordWindow());
            }
            return null;
        }

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
                else
                {
                    var frame = getFrame(frameName);
                    Frames.Add(frameName, frame);
                    container.ContainerGrid.Children.Clear();
                    container.ContainerGrid.Children.Add((UIElement)frame);
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

        public void ShowModalMessage(ContainerType containerType, MessageType messageType, string headerText, string MessageText)
        {
            if (containers.TryGetValue(containerType, out Container container))
            {
                Message message = new Message(new ViewModel.Message(headerText, MessageText, messageType));
                OpenedMessages.Push((this, containerType));
                container.ContainerGrid.Children[container.ContainerGrid.Children.Count - 1].IsEnabled = false;
                container.ContainerGrid.Children.Add(message);
            }
        }

        public void ShowModalFrame(ContainerType containerType, FrameName frameMessage)
        {
            if (containers.TryGetValue(containerType, out Container container))
            {
                container.ContainerGrid.Children[container.ContainerGrid.Children.Count - 1].IsEnabled = false;
                if (Frames.TryGetValue(frameMessage, out object value))
                {
                    container.ContainerGrid.Children.Add((UIElement)value);
                }
                else
                {
                    var frame = getFrame(frameMessage);
                    Frames.Add(frameMessage, frame);
                    container.ContainerGrid.Children.Add((UIElement)frame);
                }
            }
        }

        public void CloseModal(ContainerType containerType)
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

        static public void ClodeLastModal()
        {
            var lastMessage = OpenedMessages.Pop();
            if (lastMessage.navigator.containers.TryGetValue(lastMessage.container, out Container container))
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
