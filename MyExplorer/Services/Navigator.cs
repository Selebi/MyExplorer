using System;
using System.Collections.Generic;
using System.Linq;

namespace MyExplorer.Services
{
    internal static class Navigator
    {
        public static event Action<object> NewFrame;

        static Stack<FrameName> previousFrame = new Stack<FrameName>();
        static FrameName currentFrame = FrameName.Main;

        #region Frames

        public enum FrameName
        {
            Main,
            Settings,
            Users,
            Journal
        }

        static Dictionary<FrameName, object> Frames = new Dictionary<FrameName, object>()
        {
            {FrameName.Main, new Controls.Main(new ViewModel.Main())},
            {FrameName.Settings, new Controls.Settings(ViewModel.Settings.GetInstance())},
            {FrameName.Users, new Controls.Users(new ViewModel.Users())},
            {FrameName.Journal, new Controls.Journal(new ViewModel.Journal())}
        };

        #endregion

        public static void SetFrame(FrameName frameName)
        {
            if (Frames.TryGetValue(frameName, out object value))
            {
                NewFrame?.Invoke(value);
                previousFrame.Push(currentFrame);
                currentFrame = frameName;
            }
        }

        public static void SetPreviousFrame()
        {
            var frame = previousFrame.Pop(); // Отсылка к warframe
            if (Frames.TryGetValue(frame, out object value))
            {
                NewFrame?.Invoke(value);
                currentFrame = frame;
            }
        }
    }
}
