using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using AgileEAP.Core.Utility;

namespace AgileEAP.Core
{
    /// <summary>
    /// 全局日志类
    /// </summary>
    public class GlobalLogger
    {
        static InnerLogger innerLogger = null;
        static GlobalLogger()
        {
            innerLogger = new InnerLogger();
        }

        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            innerLogger.WriteLine(message);
        }

        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info<T>(string message)
        {
            LogManager.GetLogger(typeof(T)).Info(message);
        }

        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info<T>(string message, params object[] args)
        {
            LogManager.GetLogger(typeof(T)).Info(string.Format(message, args));
        }

        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="message"></param>
        public static void Debug<T>(string message)
        {
            LogManager.GetLogger(typeof(T)).Debug(message);
        }

        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="message"></param>
        public static void Debug<T>(string message, params object[] args)
        {
            LogManager.GetLogger(typeof(T)).Debug(string.Format(message, args));
        }

        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error<T>(string message)
        {
            LogManager.GetLogger(typeof(T)).Error(message);
        }

        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error<T>(string message, Exception exception)
        {
            LogManager.GetLogger(typeof(T)).Error(message, exception);
        }

        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error<T>(System.Exception exception)
        {
            LogManager.GetLogger(typeof(T)).Error(exception);
        }

        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error<T>(string message, params object[] args)
        {
            LogManager.GetLogger(typeof(T)).Error(string.Format(message, args));
        }

        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            innerLogger.WriteLine(message);
        }


        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="value"></param>
        public static void Error(object value)
        {
            if (value != null)
                innerLogger.WriteLine(value.ToString());
        }

        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Error(string format, params object[] args)
        {
            innerLogger.WriteLine(string.Format(format, args));
        }

        /// <summary>
        /// 写系统事件日志
        /// </summary>
        /// <param name="message"></param>
        public static void WriteEvent(string message)
        {
            innerLogger.WriteEvent(message);
        }
    }

    /// <summary>
    /// 记录本地日志
    /// </summary>
    internal class InnerLogger
    {
        string LogPath = string.Empty;
        public InnerLogger()
        {
            try
            {
                try
                {
                    string executePath = System.Web.Hosting.HostingEnvironment.IsHosted ? System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath : Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string logFilePath = Path.Combine(executePath, "Log");

                    LogPath = Path.Combine(logFilePath, "GlobalLog.txt");
                    CreateLogPath(LogPath);
                }
                catch (Exception ex)
                {
                    WriteEvent("初始化记录本地日志实例失败" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (!System.Diagnostics.EventLog.Exists("Logger"))
                    {
                        System.Diagnostics.EventLog.CreateEventSource("AgileEAP", "Logger");
                    }
                    System.Diagnostics.EventLog.WriteEntry("AgileEAP", ex.Message);
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// 写本地文件日志
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public void WriteLine(string message)
        {
            // 没有配置本地文件路径则写事件日志
            if (string.IsNullOrEmpty(LogPath))
            {
                WriteEvent(message);
                return;
            }

            FileStream fs = null;
            lock (this)
            {
                try
                {
                    // 每天一个文件
                    string filePath = LogPath;//.Replace(".", DateTime.Today.ToString("yyyyMMdd."));

                    fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                    if (fs != null)
                    {
                        //UTF8Encoding(true)
                        string msg = DateTime.Now.ToString() + "-----" + message + "\r\n";
                        //byte[] msgbyte = System.Text.Encoding.Default.GetBytes(msg);
                        byte[] msgbyte = System.Text.Encoding.UTF8.GetBytes(msg);
                        //byte[] msgbyte = new System.Text.UTF8Encoding(true).GetBytes(msg);
                        fs.Write(msgbyte, 0, msgbyte.Length);
                    }
                }
                catch (Exception ex)
                {
                    WriteEvent("写本地日志失败，失败原因：" + ex.Message + "--日志内容：" + message);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 创建日志文件目录
        /// </summary>
        /// <param name="path">文件名称</param>
        void CreateLogPath(string localPath)
        {
            if (string.IsNullOrEmpty(localPath))
            {
                WriteEvent("没有配置本地日志文件，请检查web.config配置LocalLogPath节点是否正确配置");
                return;
            }

            int index = localPath.LastIndexOf('\\');
            if (index >= 0)
            {
                string path = localPath.Remove(index + 1);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            if (!File.Exists(localPath))
            {
                using (FileStream fs = File.Create(localPath))
                { fs.Close(); }
            }
        }

        /// <summary>
        /// 写系统事件日志
        /// </summary>
        /// <param name="message"></param>
        public void WriteEvent(string message)
        {
            try
            {
                if (!System.Diagnostics.EventLog.Exists("Logger"))
                {
                    System.Diagnostics.EventLog.CreateEventSource("AgileEAP", "Logger");
                }
                System.Diagnostics.EventLog.WriteEntry("AgileEAP", message);
            }
            finally
            {
            }
        }
    }
}
