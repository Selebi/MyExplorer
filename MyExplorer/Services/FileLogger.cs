using System;
using System.Collections.Generic;
using System.Linq;

namespace MyExplorer.Services
{
    public class FileLogger
    {
        private int trynum = 1;
        public string Path { get; private set; }
        public List<(string, Enums.LogType)> Journal { get; private set; }
        public event Action<string, Enums.LogType> NewLog;

        FileLogger(string Path)
        {
            this.Path = Path;
            Journal = new List<(string, Enums.LogType)>();
        }

        #region Синглтон

        static List<FileLogger> loggers = new List<FileLogger>();

        public static FileLogger GetInstance(string Path)
        {
            if (loggers.Any(l => l.Path == Path))
            {
                return loggers.Find(l => l.Path == Path);
            }
            else
            {
                var logger = new FileLogger(Path);
                loggers.Add(logger);
                return logger;
            }
        }

        #endregion

        public void Write(string Message, Enums.LogType type)
        {
            try
            {
                using (var stream = System.IO.File.AppendText(Path))
                {
                    Message = Message.Trim();
                    stream.WriteLine($"{DateTime.Now} [{Enum.GetName(typeof(Enums.LogType), type)}] - {Message}");
                    Journal.Add((Message, type));
                    NewLog?.Invoke(Message, type);
                }
            }
            catch(Exception ex)
            {
                if(trynum <= 5)
                {
                    trynum++;
                    System.Threading.Thread.Sleep(200);
                    Write(Message, type);
                    trynum = 1;
                }
                else
                {
                    trynum = 1;
                }
            }
        }
    }
}
