using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading;

using Common.Forms;
using Common.Users;

namespace Common.Utilities
{
    public class LogItem
    {
        public LogItem(Level level, string message)
        {
            LogLevel = level;
            LogMessage = message;
        }

        public override string ToString() { return LogMessage ?? "null"; }
        public Level LogLevel { get; set; }
        public string LogMessage { get; set; }
        public Color Coloring { get { return LogLevelMapper.ColorFromLevel(LogLevel); } }
    }

    public static class LogLevelMapper
    {
        static LogLevelMapper()
        {
            LevelDictionary["None"] = Level.NONE;
            LevelDictionary["Error"] = Level.ERROR;
            LevelDictionary["Warn"] = Level.WARN;
            LevelDictionary["Info"] = Level.INFO;
            LevelDictionary["Trace"] = Level.TRACE;
            LevelColoring[Level.TRACE] = Color.FloralWhite;
            LevelColoring[Level.INFO] = Color.LightYellow;
            LevelColoring[Level.WARN] = Color.AntiqueWhite;
            LevelColoring[Level.ERROR] = Color.LightSalmon;
            LabelDictionary[Level.TRACE] = "[TRACE]: ";
            LabelDictionary[Level.INFO] = "[INFO ]: ";
            LabelDictionary[Level.WARN] = "[WARN ]: ";
            LabelDictionary[Level.ERROR] = "[ERROR]: ";
        }

        public static Level LevelFromString(string levelString)
        {
            return (LevelDictionary.ContainsKey(levelString)) ? LevelDictionary[levelString] : Level.NONE;
        }

        public static Color ColorFromLevel(Level level)
        {
            return (LevelColoring.ContainsKey(level)) ? LevelColoring[level] : Color.White;
        }

        public static string LevelString(Level level)
        {
            return (LabelDictionary.ContainsKey(level)) ? LabelDictionary[level] : "[NONE ]: ";
        }

        private static Dictionary<string, Level> LevelDictionary = new Dictionary<string, Level>();
        private static Dictionary<Level, Color> LevelColoring = new Dictionary<Level, Color>();
        private static Dictionary<Level, string> LabelDictionary = new Dictionary<Level, string>();
    }

    public enum Level
    {
        NONE = 0,
        ERROR = 1,
        WARN = 2,
        INFO = 3,
        TRACE = 4,
    }

    public class LogUtility
    {
        public LogUtility(string loggerName)
        {
            LogSource = loggerName ?? "UNKNOWN";
        }

        public void Trace(string logMessage)
        {
            LogUtilityHelper.Log(Level.TRACE, LogSource, logMessage);
        }

        public void Info(string logMessage)
        {
            LogUtilityHelper.Log(Level.INFO, LogSource, logMessage);
        }

        public void Warn(string logMessage)
        {
            LogUtilityHelper.Log(Level.WARN, LogSource, logMessage);
        }

        public void Error(string logMessage)
        {
            LogUtilityHelper.Log(Level.ERROR, LogSource, logMessage);
        }

        public bool CanPrintLevel(Level level)
        {
            return level <= LogUtilityHelper.GlobalLogLevel;
        }

        public string LogSource
        {
            get { return (_name.Length > 0) ? ProcType + _name : "UNKNOWN"; }
            set { _name = value; }
        }

        private string FindThatName()
        {
            SharedProperties prop = SharedProperties.Instance;
            if (prop.DistInstance != null && prop.DistInstance.MyProcessInfo != null)
            {
                _procType = prop.DistInstance.MyProcessInfo.Label?.Substring(0, 4);
                _procType += "# ";
            }

            return _procType ?? "NONE# ";
        }

        private string ProcType { get { return _procType ?? FindThatName(); } }

        private string _procType = null;
        private string _name = "";
        private static LogHelper LogUtilityHelper = new LogHelper();

        public bool ConsoleOutput { set { LogUtilityHelper.PrintToConsole = value; } }
        public bool FileOutput { set { LogUtilityHelper.WriteToFile = value; } }
        public bool GuiOutput { set { LogUtilityHelper.GuiOutput = value; } }
        public Level LogLevel
        {
            get { return LogUtilityHelper.GlobalLogLevel; }
            set { LogUtilityHelper.GlobalLogLevel = value; }
        }

        public void RegisterGuiCallback(BaseWindowForm winForm)
        {
            LogUtilityHelper.OnGuiLogPrint += new LogHelper.GuiLogPrintEvent(winForm.PrintLogMessage);
        }

        public void RemoveGuiCallback(BaseWindowForm winForm)
        {
            LogUtilityHelper.OnGuiLogPrint -= new LogHelper.GuiLogPrintEvent(winForm.PrintLogMessage);
        }
    }

    public class LogHelper : Threaded
    {
        public LogHelper() : base("Logger")
        {

            GlobalLogLevel = Level.TRACE;
            PrintToConsole = true;
            WriteToFile = true;
            GuiOutput = true;
            LogFileName = "Log - " + DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".txt";

            ContinueThread = true;
            Start();
        }

        public void Log(Level logLevel, string source, string message)
        {
            try
            {
                string currentTime = DateTime.Now.ToString("hh:mm:ss.s: ");
                string logMessageLine = currentTime + LogLevelMapper.LevelString(logLevel) + source + " - " + message;
                if ((int)logLevel <= (int)GlobalLogLevel) { LogQueue.Enqueue(logMessageLine); }
                if (GuiOutput) { OnGuiLogPrint?.Invoke(new LogItem(logLevel, logMessageLine)); }
            }
            catch (KeyNotFoundException e)
            {
                Log(Level.ERROR, "LogHelper", "Key from " + source + " - " + e.Message);
            }
        }

        protected override void Run()
        {
            System.IO.StreamWriter logFile = new System.IO.StreamWriter(LogFileName);

            while (ContinueThread || !LogQueue.IsEmpty)
            {
                string message;
                if (!LogQueue.IsEmpty && LogQueue.TryDequeue(out message))
                {
                    if (PrintToConsole)
                    {
                        Console.WriteLine(message);
                    }
                    if (WriteToFile)
                    {
                        logFile.WriteLine(message);
                        logFile.Flush();
                    }
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }

        public Level GlobalLogLevel { get; set; }
        public bool PrintToConsole { get; set; }
        public bool GuiOutput { get; set; }
        public bool WriteToFile { get; set; }
        public string LogFileName { get; set; }

        public delegate void GuiLogPrintEvent(LogItem message);
        public event GuiLogPrintEvent OnGuiLogPrint;

        private ConcurrentQueue<string> LogQueue = new ConcurrentQueue<string>();
    }
}
