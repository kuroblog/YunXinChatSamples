using Microsoft.Practices.Prism.Logging;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CustomControls.Samples.WPF
{
    public class NLogWrapper : ILoggerFacade
    {
        protected readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public NLogWrapper()
        {
            _logger.Debug($"{GetType().FullName} ctor.");
        }

        protected virtual void OnLog(string message, Category category, Priority priority)
        {
            //var ss = Environment.StackTrace;
            //var stackTrace = new StackTrace(true);
            //stackTrace.GetFrames().Where(a => !string.IsNullOrEmpty(a.GetFileName())).ToList().ForEach(a =>
            //{
            //    var obj = new
            //    {
            //        lineNo = a.GetFileLineNumber(),
            //        name = a.GetFileName(),
            //        colNo = a.GetFileColumnNumber(),
            //        code = a.GetHashCode(),
            //        offset = a.GetILOffset(),
            //        method = a.GetMethod(),
            //        nOffset = a.GetNativeOffset(),
            //        type = a.GetType()
            //    };

            //    var json = obj.ToFormatJson();

            //    _logger.Debug(json);
            //});

            string getNameOfCallingClass()
            {
                var skipFrames = 4;

                //Type declaringType;
                //string fullName;

                var method = new StackFrame(skipFrames, false).GetMethod();

                return $"|-{method.DeclaringType?.FullName}{Environment.NewLine}|--{method.Name} =>";

                //do
                //{
                //    var method = new StackFrame(skipFrames, false).GetMethod();

                //    declaringType = method.DeclaringType;
                //    if (declaringType == null)
                //    {
                //        return method.Name;
                //    }

                //    fullName = declaringType.FullName;

                //    skipFrames++;
                //} while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

                //return fullName;
            }

            var callName = getNameOfCallingClass();

            // method 1
            LogLevel getLogLevel(Category categoryLevel)
            {
                switch (categoryLevel)
                {
                    case Category.Exception: return LogLevel.Error;
                    case Category.Warn: return LogLevel.Warn;
                    case Category.Info: return LogLevel.Info;
                    case Category.Debug:
                    default: return LogLevel.Debug;
                }
            }

            var logLevel = getLogLevel(category);

            //_logger.Log(logLevel, message);

            //var names = new StackTrace(true).GetFrames().Where(a => !string.IsNullOrEmpty(a.GetFileName())).Select(a => a.GetMethod().ReflectedType.FullName);
            //var names = new StackTrace(true).GetFrames().Where(a => !string.IsNullOrEmpty(a.GetFileName())).Select(a => a.)?.ToArray();
            //_logger.Log(logLevel, $"{callName} {message} ... {names.ToJson()}");

            _logger.Log(logLevel, $"{callName} {message}");

            // or
            // method 2
            //switch (category)
            //{
            //    case Category.Exception:
            //        _logger.Error(message);
            //        break;
            //    case Category.Warn:
            //        _logger.Warn(message);
            //        break;
            //    case Category.Info:
            //        _logger.Info(message);
            //        break;
            //    case Category.Debug:
            //    default:
            //        _logger.Debug(message);
            //        break;
            //}
        }

        public virtual void Error(Exception error) => _logger.Error(error);

        #region ILoggerFacade
        public void Log(string message, Category category, Priority priority)
        {
            OnLog(message, category, priority);
        }
        #endregion
    }

    public static class NLogWrapperExtensions
    {
        public static void Debug(this ILoggerFacade logger, string message, Category category = Category.Debug, Priority priority = Priority.None)
        {
            if (logger is NLogWrapper nlog)
            {
                nlog.Log(message, category, priority);
            }
            else
            {
                logger?.Log(message, category, priority);
            }
        }

        public static void Info(this ILoggerFacade logger, string message, Category category = Category.Info, Priority priority = Priority.None)
        {
            if (logger is NLogWrapper nlog)
            {
                nlog.Log(message, category, priority);
            }
            else
            {
                logger?.Log(message, category, priority);
            }
        }

        public static void Warn(this ILoggerFacade logger, string message, Category category = Category.Warn, Priority priority = Priority.None)
        {
            if (logger is NLogWrapper nlog)
            {
                nlog.Log(message, category, priority);
            }
            else
            {
                logger?.Log(message, category, priority);
            }
        }

        public static void Error(this ILoggerFacade logger, string message, Category category = Category.Exception, Priority priority = Priority.None)
        {
            if (logger is NLogWrapper nlog)
            {
                nlog.Log(message, category, priority);
            }
            else
            {
                logger?.Log(message, category, priority);
            }
        }

        public static void Error(this ILoggerFacade logger, Exception error)
        {
            if (logger is NLogWrapper nlog)
            {
                nlog.Error(error);
            }
            else
            {
                try
                {
                    logger?.Error(error.ToErrorText());
                }
                catch (Exception e)
                {
                    logger?.Error(e);
                }
            }
        }
    }

    public class CustomErrorLog
    {
        public string[] Data { get; set; }

        public string Link { get; set; }

        public int Code { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }

        public string MethodName { get; set; }
    }

    public static class CustomErrorLogExtensions
    {
        public static string ToErrorText(this Exception error, bool isFormat = false)
        {
            string[] getFormatErrors(IDictionary dict, string format = "{0}:{1}")
            {
                var result = dict
                    ?.Keys.Cast<object>()
                    .ToDictionary(k => k.ToString(), v => dict[v])
                    .Select(a => string.Format(format, a.Key, a.Value))
                    ?.ToArray();

                return result;
            }

            var errorLogs = new List<CustomErrorLog>();

            while (error != null)
            {
                var errorLog = new CustomErrorLog
                {
                    Data = getFormatErrors(error.Data),
                    Link = error.HelpLink,
                    Message = error.Message,
                    Source = error.Source,
                    StackTrace = error.StackTrace,
                    MethodName = error.TargetSite?.Name
                };

                //errorLogs.Insert(0, errorLog);

                errorLogs.Add(errorLog);

                error = error.InnerException;
            }

            var errorText = JsonConvert.SerializeObject(errorLogs, isFormat ? Formatting.Indented : Formatting.None);

            return errorText;
        }
    }

    public static class JsonExtensions
    {
        public static string ToJson<T>(this T value, bool formatJson = false)
        {
            return JsonConvert.SerializeObject(value, formatJson ? Formatting.Indented : Formatting.None);
        }

        public static string ToFormatJson<T>(this T value) => value.ToJson(true);

        public static T ToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
