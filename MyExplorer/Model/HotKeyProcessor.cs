using MyExplorer.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.Model
{
    public static class HotkeyProcessor
    {
        static string masterKey = "A+D+M+N";

        static ViewModel.Settings settings = ViewModel.Settings.GetInstance();
        static FileLogger fl = FileLogger.GetInstance(ViewModel.Settings.GetInstance().LogFile);
        static List<string> keys = new List<string>();

        static public event Action<string> CurrentHotKeyChanged;
        static public event Action<bool> LockedStatusChanged;
        static public event Action<bool> LockedAllStatusChanged;
        static public event Action MasterKeyDetected;

        static bool _lockedAll = false;
        static public bool LockedAll
        {
            get => _lockedAll;
            set
            {
                _lockedAll = value;
                LockedAllStatusChanged?.Invoke(_lockedAll);
            }
        }

        static bool _locked = true;
        static public bool Locked
        {
            get => _locked;
            set
            {
                _locked = value;
                LockedStatusChanged?.Invoke(_locked);
            }
        }


        static void Out()
        {
            string keysStr = "";
            keys.ForEach(k => { keysStr += $"{k}+"; });
            keysStr = keysStr.Trim(new char[] { '+' });
            if (keysStr == "")
            {
                Debug.WriteLine("");
                CurrentHotKeyChanged?.Invoke("");
            }
            else
            {
                Debug.WriteLine(keysStr);
                CurrentHotKeyChanged?.Invoke(keysStr);

                if(keysStr == masterKey)
                {
                    MasterKeyDetected?.Invoke();
                }
            }
        }

        public static bool AddKey(int keycode)
        {
            string key = Decode(keycode);

            if (key == "NaN") return false;

            if (keys.Count == 0 || key != keys[keys.Count - 1])
            {
                if (keys.Count == 4)
                {
                    keys.RemoveAt(0);
                }
                keys.Add(key);
            }

            Out();

            if (!LockedAll && !Locked)
                return false;
            if (LockedAll || Check())
            {
                Debug.WriteLine("Goth!");
                return true;
            }
            return false;
        }

        public static void RemoveKey(int keycode)
        {
            string key = Decode(keycode);

            if (key == "NaN") return;

            keys.RemoveAll(k => k == key);

            Out();
        }

        public static void RemoveAllKeys()
        {
            keys.Clear();

            Out();
        }

        static string Decode(int keycode)
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
                    return "NaN";
                }
            }
            return buf;
        }

        // Адовый алгоритм
        static bool Check()
        {
            foreach (var h in settings.HotkeysEnds)
            {
                if (keys[keys.Count - 1] == h)
                {
                    bool containsFlag;
                    foreach (var hke in settings.HotkeysElements)
                    {
                        containsFlag = true;
                        foreach (var hk in hke)
                        {
                            if (!keys.Contains(hk))
                            {
                                containsFlag = false;
                                break;
                            }
                        }
                        if (containsFlag)
                        {
                            return true;
                        }
                    }
                }
            }
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
            { 48, "0" },
            { 49, "1" },
            { 50, "2" },
            { 51, "3" },
            { 52, "4" },
            { 53, "5" },
            { 54, "6" },
            { 55, "7" },
            { 56, "8" },
            { 57, "9" },
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
            { 106, "*" },
            { 107, "+" },
            { 109, "-" },
            { 110, "." },
            { 111, "/" },
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
            { 145, "ScrLk" },
            { 160, "Shift" },
            { 161, "RShift" },
            { 162, "Ctrl" },
            { 163, "RCtrl" },
            { 164, "Alt" },
            { 165, "RAlt" },
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
