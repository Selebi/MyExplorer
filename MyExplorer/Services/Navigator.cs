﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyExplorer.Services
{
    internal class Navigator
    {
        public WindowName currentWindow = WindowName.Null;
        public Grid currentgrid;
        static List<Navigator> navigators = new List<Navigator>();

        Navigator() { }

        public static Navigator GetInstance(WindowName window)
        {
            if (navigators.Any(n => { return n.currentWindow == window; }))
                return navigators.Find(n => { return n.currentWindow == window; });
            else
                return null;
        }

        public static void CreateInstance(WindowName window, Grid container)
        {
            if (navigators.Any(n => { return n.currentWindow == window; }))
            {
                navigators.RemoveAll(n => { return n.currentWindow == window; });
            }
            navigators.Add(new Navigator() { currentWindow = window, currentgrid = container });
        }

        Stack<FrameName> previousFrame = new Stack<FrameName>();
        FrameName currentContent = FrameName.Main;

        #region Frames

        public enum FrameName
        {
            Main,
            Settings,
            Users,
            Journal,
            Hotkeys,
            AddHotKey
        }

        public enum WindowName
        {
            Null,
            Settings,
            Process
        }

        static Dictionary<FrameName, object> Frames = new Dictionary<FrameName, object>()
        {
            {FrameName.Main, new Controls.Main(new ViewModel.Main())},
            {FrameName.Settings, new Controls.Settings(ViewModel.Settings.GetInstance())},
            {FrameName.Users, new Controls.Users(new ViewModel.Users())},
            {FrameName.Journal, new Controls.Journal(new ViewModel.Journal())},
            {FrameName.Hotkeys, new Controls.HotKeys(new ViewModel.HotKeys())},
            {FrameName.AddHotKey, new Controls.AddHotKey(new ViewModel.AddHotKey())}
        };

        #endregion

        public void SetFrame(FrameName frameName)
        {
            if (Frames.TryGetValue(frameName, out object value))
            {
                currentgrid.Children.Clear();
                currentgrid.Children.Add((System.Windows.UIElement)value);
                previousFrame.Push(currentContent);
                currentContent = frameName;
            }
        }

        public void SetPreviousFrame()
        {
            var frame = previousFrame.Pop(); // Отсылка к warframe
            if (Frames.TryGetValue(frame, out object value))
            {
                currentgrid.Children.Clear();
                currentgrid.Children.Add((System.Windows.UIElement)value);
                currentContent = frame;
            }
        }

        public void ShowMessage(FrameName frameName)
        {
            if (Frames.TryGetValue(frameName, out object value))
            {
                currentgrid.Children[currentgrid.Children.Count - 1].IsEnabled = false;
                currentgrid.Children.Add((System.Windows.UIElement)value);
            }
        }

        public void CloseMessage()
        {
            if (currentgrid.Children.Count >= 2)
            {
                currentgrid.Children.RemoveAt(currentgrid.Children.Count - 1);
                currentgrid.Children[currentgrid.Children.Count - 1].IsEnabled = true;
            }
        }
    }
}
