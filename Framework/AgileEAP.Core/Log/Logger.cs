using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core
{
    public class Logger : AgileEAP.Core.ILogger
    {
        log4net.ILog logger = null;

        public Logger(string loggerName)
        {
            logger = log4net.LogManager.GetLogger(loggerName);
        }

        public Logger(Type type)
        {
            logger = log4net.LogManager.GetLogger(type);
        }

        public void Error(object message)
        {
            logger.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            logger.Error(message, exception);
        }

        public void ErrorFormat(string format, object arg0)
        {
            logger.ErrorFormat(format, arg0);
        }

        public void Warn(object message)
        {
            logger.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            logger.Warn(message, exception);
        }

        public void Info(object message)
        {
            logger.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            logger.Info(message, exception);
        }

        public void Fatal(object message)
        {
            logger.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            logger.Fatal(message, exception);
        }

        public void Debug(object message)
        {
            logger.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            logger.Debug(message, exception);
        }

        public void DebugFormat(string format, object arg0)
        {
            logger.DebugFormat(format, arg0);
        }

        public void ActionLog<T>(ActionType actionType, DoResult actionResult, string message) where T : class
        {
            logger.Error(message);
        }

        public void ActionLog(Type type, ActionType actionType, DoResult actionResult, string message)
        {
            logger.Error(message);
        }

        public void ActionLog<T>(string keyWord, string message) where T : class
        {
            logger.Error(message);
        }

        public void ActionLog<T>(string message) where T : class
        {
            logger.Error(message);
        }

        public void ActionLog(string module, string keyWord, string message)
        {
            logger.Error(message);
        }

        public void ActionLog(string module, string message)
        {
            logger.Error(message);
        }
    }
}
