using MyExplorer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.Model
{
    public static class HotkeyProcessor
    {
        static ViewModel.Settings settings = ViewModel.Settings.GetInstance();
        static FileLogger fl = FileLogger.GetInstance(ViewModel.Settings.GetInstance().LogFile);
        static List<string> keys = new List<string>();

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
                else
                {
                    fl.Write($"Нажата неизвестная клавиша. Код - {keycode}", Enums.LogType.Warning);
                    return;
                }
            }

            if (keys.Count == 0 || buf != keys[keys.Count - 1])
            {
                if (keys.Count == 4)
                {
                    keys.RemoveAt(0);
                }
                keys.Add(buf);
            }

            if (Check())
            {

            }
        }

        static bool Check()
        {
            return false;
        }

        static Dictionary<int, string> Keykeys = new Dictionary<int, string>()
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
    }
}
