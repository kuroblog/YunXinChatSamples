using Microsoft.Practices.Prism.Logging;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Desktop.Samples.Common
{
    public class NLogWrapper : ILoggerFacade
    {
        protected readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public NLogWrapper()
        {
            _logger?.Debug($"{GetType().Name} ... ctor.");
        }

        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Exception:
                    _logger?.Error(message);
                    break;
                case Category.Warn:
                    _logger?.Warn(message);
                    break;
                case Category.Info:
                    _logger?.Info(message);
                    break;
                case Category.Debug:
                default:
                    _logger?.Debug(message);
                    break;
            }
        }

        public void Error(Exception error)
        {
            _logger.Error(error);
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

    public static class NLogWrapperExtensions
    {
        public static void Debug(this ILoggerFacade logger, string message)
        {
            if (logger is NLogWrapper nlogger)
            {
                nlogger.Log(message, Category.Debug, Priority.None);
            }
            else
            {
                logger?.Log(message, Category.Debug, Priority.None);
            }
        }

        public static void Info(this ILoggerFacade logger, string message)
        {
            if (logger is NLogWrapper nlogger)
            {
                nlogger.Log(message, Category.Info, Priority.None);
            }
            else
            {
                logger?.Log(message, Category.Info, Priority.None);
            }
        }

        public static void Warn(this ILoggerFacade logger, string message)
        {
            if (logger is NLogWrapper nlogger)
            {
                nlogger.Log(message, Category.Warn, Priority.None);
            }
            else
            {
                logger?.Log(message, Category.Warn, Priority.None);
            }
        }

        public static void Error(this ILoggerFacade logger, string message)
        {
            if (logger is NLogWrapper nlogger)
            {
                nlogger.Log(message, Category.Exception, Priority.None);
            }
            else
            {
                logger?.Log(message, Category.Exception, Priority.None);
            }
        }

        public static string[] ToFormatArray(this IDictionary dict, string format = "{0}:{1}")
        {
            return dict?
                .Keys.Cast<object>()
                .ToDictionary(k => k.ToString(), v => dict[v])
                .Select(a => string.Format(format, a.Key, a.Value))?
                .ToArray();
        }

        public static string ToErrorText(this Exception error, bool isFormat = false)
        {
            var errorLogs = new List<CustomErrorLog>();

            while (error != null)
            {
                var errorLog = new CustomErrorLog
                {
                    Data = error.Data?.ToFormatArray(),
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

            return JsonConvert.SerializeObject(errorLogs, isFormat ? Formatting.Indented : Formatting.None);
        }

        public static void Error(this ILoggerFacade logger, Exception error)
        {
            if (logger is NLogWrapper nlogger)
            {
                nlogger.Error(error);
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
}
