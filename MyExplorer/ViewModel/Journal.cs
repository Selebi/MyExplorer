﻿using System;

namespace MyExplorer.ViewModel
{
    public class Journal : BaseVM
    {
        public Journal()
        {
            var fl = Services.FileLogger.GetInstance(Settings.GetInstance().LogFile);
            fl.Journal.ForEach((s) =>
            {
                JournalText += $"{DateTime.Now} [{Enum.GetName(typeof(Enums.LogType), s.Item2)}] - {s.Item1}\r\n";
            });
            fl.NewLog += Fl_NewLog;
        }

        private void Fl_NewLog(string message, Enums.LogType type)
        {
            JournalText += $"{DateTime.Now} [{Enum.GetName(typeof(Enums.LogType), type)}] - {message}\r\n";
        }

        public RelayCommand Back
        {
            get => new RelayCommand((o) =>
            {
                Services.Navigator.GetInstance(Enums.WindowName.Settings).SetPreviousFrame(Enums.ContainerType.Main);
            });
        }

        string _journalText;
        public string JournalText
        {
            get => _journalText;
            set
            {
                _journalText = value;
                OnPropertyChanged();
            }
        }
    }
}
