using System;

namespace AgileEAP.Core
{
    public interface ILogger
    {
        void ActionLog(string module, string keyWord, string message);
        void ActionLog(string module, string message);
        void ActionLog(Type type, ActionType actionType, DoResult actionResult, string message);
        void ActionLog<T>(ActionType actionType, DoResult actionResult, string message) where T : class;
        void ActionLog<T>(string keyWord, string message) where T : class;
        void ActionLog<T>(string message) where T : class;
        void Debug(object message);
        void Debug(object message, Exception exception);
        void DebugFormat(string format, object arg0);
        void Error(object message);
        void Error(object message, Exception exception);
        void Fatal(object message);
        void Fatal(object message, Exception exception);
        void Info(object message);
        void Info(object message, Exception exception);
        void Warn(object message);
        void Warn(object message, Exception exception);
    }
}
