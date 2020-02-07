using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MyExplorer.Services
{
    public class HotkeyLocker
    {
        public HotkeyLocker()
        {
            settings = ViewModel.Settings.GetInstance();
        }

        private ViewModel.Settings settings;

        private const int WH_KEYBOARD_LL = 13;

        private LowLevelKeyboardProcDelegate m_callback;

        private IntPtr m_hHook;


        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, int dwThreadId);


        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);


        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(IntPtr lpModuleName);


        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        private static Dictionary<int, string> Keykeys = new Dictionary<int, string>()
        {
            { 8, "BackSpace" },
            { 9, "Tab" },
            { 13, "Enter" },
            { 19, "Pause" },
            { 20, "CapsLock" },
            { 27, "Esc" },
            { 32, "Space" },
            { 33, "PageUp" },
            { 34, "PageDown" },
            { 35, "End" },
            { 36, "Home" },
            { 37, "Left" },
            { 38, "Up" },
            { 39, "Right" },
            { 40, "Down" },
            { 44, "PrtSc" },
            { 45, "Insert" },
            { 46, "Delete" },
            { 91, "Win" },
            { 93, "Menu" },
            { 96, "Num0" },
            { 97, "Num1" },
            { 98, "Num2" },
            { 99, "Num3" },
            { 100, "Num4" },
            { 101, "Num5" },
            { 102, "Num6" },
            { 103, "Num7" },
            { 104, "Num8" },
            { 105, "Num9" },
            { 107, "+" },
            { 109, "-" },
            { 112, "F1" },
            { 113, "F2" },
            { 114, "F3" },
            { 115, "F4" },
            { 116, "F5" },
            { 117, "F6" },
            { 118, "F7" },
            { 119, "F8" },
            { 120, "F9" },
            { 121, "F10" },
            { 122, "F11" },
            { 123, "F12" },
            { 144, "Num" },
            { 160, "Shift" },
            { 161, "Shift" },
            { 162, "Ctrl" },
            { 163, "Ctrl" },
            { 164, "Alt" },
            { 165, "Alt" },
            { 186, ";" },
            { 187, "=" },
            { 188, "<" },
            { 189, "_" },
            { 190, ">" },
            { 191, "?" },
            { 192, "~" },
            { 219, "[" },
            { 220, "\\" },
            { 221, "]" },
            { 222, "\"" },
        };

        static class hotkeyState 
        {
            static Stack<string> keys = new Stack<string>(4);
            public static void AddKey(int keycode)
            {
                string buf = "";
                if (keycode >= 65 && keycode <= 90)
                    buf += (char)keycode;
                else
                {
                    if (Keykeys.TryGetValue(keycode, out string val))
                    {
                        buf = val;
                    }
                }
                if (keys.Count == 0 || buf != keys.Peek())
                    keys.Push(buf);
            }

            public static string GetKeys()
            {
                return "";
            }
        }

        private IntPtr LowLevelKeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0 || wParam.ToInt32() == 257 || wParam.ToInt32() == 261) return CallNextHookEx(m_hHook, nCode, wParam, lParam);
            else
            {
                var key = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                int KeyCode = (key).VirtualKeyCode;
                hotkeyState.AddKey(KeyCode);
                if (true) 
                {
                    return new IntPtr(1);
                }
                else
                {
                    return CallNextHookEx(m_hHook, nCode, wParam, lParam);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardHookStruct
        {
            public readonly int VirtualKeyCode;
            public readonly int ScanCode;
            public readonly int Flags;
            public readonly int Time;
            public readonly IntPtr ExtraInfo;
        }

        private delegate IntPtr LowLevelKeyboardProcDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        public void SetHook()
        {
            m_callback = LowLevelKeyboardHookProc;
            m_hHook = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback, GetModuleHandle(IntPtr.Zero), 0);
        }

        public void Unhook()
        {
            UnhookWindowsHookEx(m_hHook);
        }
    }
}