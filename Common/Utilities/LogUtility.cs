using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;

namespace Common.Utilities
{
    public class LogItem
    {
        public LogItem(Level level, string message)
        {
            LogLevel = level;
            LogMessage = message;
        }

        public Level LogLevel { get; set; }
        public string LogMessage { get; set; }

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
        }

        public static Level LevelFromString(string levelString)
        {
            Level result = Level.NONE;
            if (LevelDictionary.ContainsKey(levelString))
            {
                result = LevelDictionary[levelString];
            }
            return result;
        }

        private static Dictionary<string, Level> LevelDictionary = new Dictionary<string, Level>();
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
            if (loggerName == null)
            {
                loggerName = "UNKNOWN";
            }

            LogSource = loggerName;
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

        public string LogSource
        {
            get { return _name.Length > 0 ? _name : "UNKNOWN"; }
            set { _name = value; }
        }

        private string _name = "";
        private static LogHelper LogUtilityHelper = new LogHelper();

        public bool ConsoleOutput
        {
            set
            {
                LogUtilityHelper.PrintToConsole = value;
            }
        }
        public bool FileOutput
        {
            set
            {
                LogUtilityHelper.WriteToFile = value;
            }
        }
        public bool GuiOutput
        {
            set
            {
                LogUtilityHelper.GuiOutput = value;
            }
        }
        public Level LogLevel
        {
            get
            {
                return LogUtilityHelper.GlobalLogLevel;
            }
            set
            {
                LogUtilityHelper.GlobalLogLevel = value;
            }
        }
    }

    public class LogHelper
    {
        public LogHelper()
        {
            LabelDictionary = new Dictionary<Level, string>();
            LabelDictionary[Level.TRACE] = "[TRACE]: ";
            LabelDictionary[Level.INFO] = "[INFO ]: ";
            LabelDictionary[Level.WARN] = "[WARN ]: ";
            LabelDictionary[Level.ERROR] = "[ERROR]: ";

            GlobalLogLevel = Level.TRACE;
            PrintToConsole = true;
            WriteToFile = true;
            GuiOutput = true;
            LogFileName = "Log - " + DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".txt";

            ContinueThread = true;
            LogThread = new Thread(Run);
            LogThread.IsBackground = true;
            LogThread.Start();
        }

        ~LogHelper()
        {
            StopLogging();
        }

        public void StopLogging()
        {
            if (ContinueThread || LogThread.IsAlive)
            {
                ContinueThread = false;
                LogThread.Join(1000);
            }
        }

        public void Log(Level logLevel, string source, string message)
        {
            try
            {
                string currentTime = DateTime.Now.ToString("hh:mm:ss.s: ");
                string logMessageLine = currentTime + LabelDictionary[logLevel] + source + " - " + message;
                if ((int)logLevel <= (int)GlobalLogLevel)
                {
                    LogQueue.Enqueue(logMessageLine);
                }
                if (GuiOutput)
                {
                    MainWindow.GuiLogQueue.Enqueue(new LogItem(logLevel, logMessageLine));
                }
            }
            catch (KeyNotFoundException e)
            {
                Log(Level.ERROR, "LogHelper", "Key from " + source + " - " + e.Message);
            }
        }

        private void Run()
        {
            System.IO.StreamWriter logFile = new System.IO.StreamWriter(LogFileName);

            while (ContinueThread || !LogQueue.IsEmpty)
            {
                if (LogQueue.IsEmpty)
                {
                    Thread.Sleep(50);
                }
                else
                {
                    string message;
                    if (LogQueue.TryDequeue(out message))
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
                }
            }
        }

        public bool IsActive()
        {
            return ContinueThread;
        }

        public Level GlobalLogLevel { get; set; }
        public delegate
        public bool PrintToConsole { get; set; }
        public bool GuiOutput { get; set; }
        public bool WriteToFile { get; set; }
        public string LogFileName { get; set; }

        private ConcurrentQueue<string> LogQueue = new ConcurrentQueue<string>();
        private volatile bool ContinueThread = false;
        private Thread LogThread;
        private Dictionary<Level, string> LabelDictionary;
    }
}
