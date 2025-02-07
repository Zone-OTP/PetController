using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetController;

namespace PetController
{

    public class PetEventArgs : EventArgs
    {
        public Pets Pet { get; }
        public string Message { get; }
        public LogLevel Level { get; }

        public PetEventArgs(Pets pet, string message, LogLevel level = LogLevel.Info)
        {
            Pet = pet;
            Message = message;
            Level = level;
        }
    }
    public class EventLogger
    {
        public EventLogger(PetManager petManager)
        {
            petManager.PetEvent += LogEvent;
        }

        private void LogEvent(object sender, PetEventArgs e)
        {
            string logLevelString = e.Level.ToString().ToUpper();
            string logEntry = $"[{logLevelString}] {DateTime.Now} - {e.Message}";
            Console.WriteLine(logEntry);
            File.AppendAllText("log.txt", logEntry);
        }
    }
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Debug
    }


}
