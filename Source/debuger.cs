using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace CrownGames
{
    public class Debug
    {
        private static Logger m_currentLogger = LogManager.GetCurrentClassLogger();
        public static void Initialize()
        {
            LoggingConfiguration configuration = new LoggingConfiguration();
            ColoredConsoleTarget colorTarget = new ColoredConsoleTarget();
            FileTarget infoLogTarget = new FileTarget();
            FileTarget warnLogTarget = new FileTarget();
            FileTarget errorLogTarget = new FileTarget();
            configuration.AddTarget("console", colorTarget);
            colorTarget.Layout = @"${date:format=HH\:mm\:ss.fff} | ${level:padding=5} | ${message} ${exception:format=toString}";
            colorTarget.WordHighlightingRules.Add(
            new ConsoleWordHighlightingRule
            {
                Regex = @"\|\s([A-Za-z0-9\-]+)?:([A-Za-z0-9\-]+)?:([0-9\-]+)?\s\|",
                ForegroundColor = ConsoleOutputColor.Green
            });
            configuration.AddTarget("file", infoLogTarget);
            infoLogTarget.FileName = "logs/Log.Info.log";
            infoLogTarget.Layout = "${longdate} | ${level:padding=5} | ${message}";
            configuration.AddTarget("file", warnLogTarget);
            warnLogTarget.FileName = "logs/Log.Warning.log";
            warnLogTarget.Layout = "${longdate} | ${level:padding=5} | ${message}";
            configuration.AddTarget("file", errorLogTarget);
            errorLogTarget.FileName = "logs/Log.Error.log";
            errorLogTarget.Layout = "${longdate} | ${level:padding=5} | ${message}";
            LoggingRule colorRule = new LoggingRule("*", LogLevel.Debug, colorTarget);
            LoggingRule infoRule = new LoggingRule("*", LogLevel.Debug, infoLogTarget);
            LoggingRule warnRule = new LoggingRule("*", LogLevel.Warn, warnLogTarget);
            LoggingRule errorRule = new LoggingRule("*", LogLevel.Error, errorLogTarget);
            configuration.LoggingRules.Add(colorRule);
            configuration.LoggingRules.Add(infoRule);
            configuration.LoggingRules.Add(warnRule);
            configuration.LoggingRules.Add(errorRule);
            LogManager.Configuration = configuration;
        }
        public static void Log(object _obj, [CallerLineNumber] int _sourceLineNumber = 0)
        {
#if (DEBUG)
            m_currentLogger.Info(CreateString(new StackTrace().GetFrame(1).GetMethod(), _sourceLineNumber) + _obj);
#endif
        }
        public static void Log(string _obj, Object _context, [CallerLineNumber] int _sourceLineNumber = 0)
        {
#if DEBUG
            m_currentLogger.Info(CreateString(new StackTrace().GetFrame(1).GetMethod(), _sourceLineNumber) + _obj, _context);
#endif
        }
        public static void Warning(object _obj, [CallerLineNumber] int _sourceLineNumber = 0)
        {
#if (DEBUG)
            m_currentLogger.Warn(CreateString(new StackTrace().GetFrame(1).GetMethod(), _sourceLineNumber) + _obj);
#endif
        }
        public static void Warning(string _obj, Object _context, [CallerLineNumber] int _sourceLineNumber = 0)
        {
#if DEBUG
            m_currentLogger.Warn(CreateString(new StackTrace().GetFrame(1).GetMethod(), _sourceLineNumber) + _obj, _context);
#endif
        }
        public static void Error(object _obj, [CallerLineNumber] int _sourceLineNumber = 0)
        {
            m_currentLogger.Error(CreateString(new StackTrace().GetFrame(1).GetMethod(), _sourceLineNumber) + _obj);
        }
        public static void Error(string _obj, Object _context, [CallerLineNumber] int _sourceLineNumber = 0)
        {
            m_currentLogger.Error(CreateString(new StackTrace().GetFrame(1).GetMethod(), _sourceLineNumber) + _obj, _context);
        }
        public static void Exception(Exception _exception, [CallerLineNumber] int _sourceLineNumber = 0)
        {
            m_currentLogger.Error(CreateString(new StackTrace().GetFrame(1).GetMethod(), _sourceLineNumber) + _exception);
        }
        private static string CreateString(MethodBase _methodBase, int _line)
        {
            return _methodBase.DeclaringType.Name + ":" + _methodBase.Name + ":" + _line + " | ";
        }
    }
}