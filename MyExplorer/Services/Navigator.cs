using MyExplorer.Enums;
using MyExplorer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyExplorer.Services
{
    public class Navigator
    {
        WindowName currentWindow = WindowName.Null;
        Grid currentContainer;
        Window Window;

        Stack<FrameName> previousFrame = new Stack<FrameName>();
        FrameName currentContent = FrameName.Main;

        static List<Navigator> navigators = new List<Navigator>();

        Navigator() { }

        public static Navigator GetInstance(WindowName window)
        {
            if (navigators.Any(n => { return n.currentWindow == window; }))
                return navigators.Find(n => { return n.currentWindow == window; });
            else
                return null;
        }

        //public static void CreateInstance(WindowName windowName, Window window, Grid container)
        //{
        //    if (navigators.Any(n => { return n.currentWindow == windowName; }))
        //    {
        //        navigators.RemoveAll(n => { return n.currentWindow == windowName; });
        //    }
        //    navigators.Add(new Navigator(window) { currentWindow = windowName, currentgrid = container });
        //}

        public static void CreateInstance(WindowName windowName)
        {
            if (navigators.Any(n => { return n.currentWindow == windowName; }))
            {
                navigators.RemoveAll(n => { return n.currentWindow == windowName; });
            }
            if (Windows.TryGetValue(windowName, out object window))
            {
                Navigator navigator = new Navigator();
                navigator.currentWindow = windowName;
                navigator.currentContainer = ((IExplorerWindow)window).ContainerFrame;
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
            {FrameName.Process, new Controls.Process(ViewModel.Process.GetInstance())}
        };

        static Dictionary<WindowName, object> Windows = new Dictionary<WindowName, object>()
        {
            { WindowName.Settings, new SettingWindow(new ViewModel.SettingWindow()) },
            { WindowName.Process, new ProcessWindow(new ViewModel.ProcessWindow()) },
            { WindowName.Icons, new IconsWindow(new ViewModel.IconsWindow()) },
            { WindowName.Password, new PasswordWindow(new ViewModel.PasswordWindow()) }
        };

        #endregion

        public void ReleaseFrames()
        {
            currentContainer.Children.Clear();
        }

        public void SetFrame(FrameName frameName)
        {
            if (Frames.TryGetValue(frameName, out object value))
            {
                currentContainer.Children.Clear();
                currentContainer.Children.Add((System.Windows.UIElement)value);
                previousFrame.Push(currentContent);
                currentContent = frameName;
            }
        }

        public void SetPreviousFrame()
        {
            var frame = previousFrame.Pop(); // Отсылка к warframe
            if (Frames.TryGetValue(frame, out object value))
            {
                currentContainer.Children.Clear();
                currentContainer.Children.Add((System.Windows.UIElement)value);
                currentContent = frame;
            }
        }

        public void ShowMessage(FrameName frameName)
        {
            if (Frames.TryGetValue(frameName, out object value))
            {
                currentContainer.Children[currentContainer.Children.Count - 1].IsEnabled = false;
                currentContainer.Children.Add((System.Windows.UIElement)value);
            }
        }

        public void CloseMessage()
        {
            if (currentContainer.Children.Count >= 2)
            {
                currentContainer.Children.RemoveAt(currentContainer.Children.Count - 1);
                currentContainer.Children[currentContainer.Children.Count - 1].IsEnabled = true;
            }
        }
    }
}
