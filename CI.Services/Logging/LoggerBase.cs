using System;
using System.Globalization;
using log4net;
using log4net.Core;
using log4net.Repository;
using log4net.Util;
using Savchin.Logging;
using ILogger = log4net.Core.ILogger;

namespace Savchin.Services.Logging
{
    public class LoggerBase : Savchin.Logging.ILogger
    {
        private static readonly Type ThisDeclaringType = typeof(LoggerBase);

        private Level _levelDebug;
        private Level _levelInfo;
        protected Level LevelWarn { get; private set; }
        private Level _levelError;
        private Level _levelFatal;
        private Level _levelTrace;

        private ILog log;
        protected ILogger Logger { get; }

        public bool IsWarningEnabled => Logger.IsEnabledFor(LevelWarn);

        public bool IsDebugEnabled => Logger.IsEnabledFor(_levelDebug);

        public bool IsTraceEnabled => Logger.IsEnabledFor(_levelTrace);

        public bool IsInfoEnabled => Logger.IsEnabledFor(_levelInfo);

        public bool IsWarnEnabled => Logger.IsEnabledFor(LevelWarn);

        public bool IsErrorEnabled => Logger.IsEnabledFor(_levelError);

        public bool IsFatalEnabled => Logger.IsEnabledFor(_levelFatal);


        public LoggerBase(string name)
        {
            log = LogManager.GetLogger(name);
            Logger = log.Logger;
            Logger.Repository.ConfigurationChanged += LoggerRepositoryConfigurationChanged;
            ReloadLevels(Logger.Repository);
        }

        public void Debug(object value)
        {
            Logger.Log(ThisDeclaringType, _levelDebug, value, null);
        }

        public void Debug(string message)
        {
            Logger.Log(ThisDeclaringType, _levelDebug, message, null);
        }

        public void Debug(string message, Exception exception)
        {
            Logger.Log(ThisDeclaringType, _levelDebug, message, exception);
        }

        public void Debug(string format, params object[] arguments)
        {
            if (IsDebugEnabled)
                Logger.Log(ThisDeclaringType, _levelDebug, new LoggerStringFormat(CultureInfo.InvariantCulture, format, arguments), null);
        }

        public void DebugFormat(string format, object arg0)
        {
            if (IsDebugEnabled)
                Logger.Log(ThisDeclaringType, _levelDebug, new LoggerStringFormat(CultureInfo.InvariantCulture, format, arg0), null);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            if (IsDebugEnabled)
                Logger.Log(ThisDeclaringType, _levelDebug, new LoggerStringFormat(CultureInfo.InvariantCulture, format, arg0, arg1), null);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            if (IsDebugEnabled)
                Logger.Log(ThisDeclaringType, _levelDebug, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0, arg1, arg2), null);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] arguments)
        {
            if (IsDebugEnabled)
                Logger.Log(ThisDeclaringType, _levelDebug, new SystemStringFormat(provider, format, arguments), null);
        }

        public virtual void Info(string message)
        {
            Logger.Log(ThisDeclaringType, _levelInfo, message, null);
        }

        public void Info(string message, Exception exception)
        {
            Logger.Log(ThisDeclaringType, _levelInfo, message, exception);
        }

        public void Info(string format, params object[] arguments)
        {
            if (IsInfoEnabled)
                Logger.Log(ThisDeclaringType, _levelInfo, new SystemStringFormat(CultureInfo.InvariantCulture, format, arguments), null);
        }

        public void Warning(Exception exception)
        {
            if (IsWarnEnabled)
                Logger.Log(ThisDeclaringType, LevelWarn, exception.ToString(), exception);
        }

        public void Warning(string message, Exception exception)
        {
            if (IsWarnEnabled)
                Logger.Log(ThisDeclaringType, LevelWarn, message, exception);
        }

        public void InfoFormat(string format, object arg0)
        {
            if (IsInfoEnabled)
                Logger.Log(ThisDeclaringType, _levelInfo, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0), null);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            if (IsInfoEnabled)
                Logger.Log(ThisDeclaringType, _levelInfo, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0, arg1), null);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            if (IsInfoEnabled)
                Logger.Log(ThisDeclaringType, _levelInfo, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0, arg1, arg2), null);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] arguments)
        {
            if (IsInfoEnabled)
                Logger.Log(ThisDeclaringType, _levelInfo, new SystemStringFormat(provider, format, arguments), null);
        }

        public void Trace(string message)
        {
            Logger.Log(ThisDeclaringType, _levelTrace, message, null);
        }

        /// <summary>
        /// Traces the specified format string.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arguments">The arguments.</param>
        public void Trace(string format, params object[] arguments)
        {
            if (IsTraceEnabled)
                Logger.Log(ThisDeclaringType, _levelTrace, new SystemStringFormat(CultureInfo.InvariantCulture, format, arguments), null);
        }


        public void Warning(string text)
        {
            Logger.Log(ThisDeclaringType, LevelWarn, text, null);
        }

        public void Warn(string message, Exception exception)
        {
            Logger.Log(ThisDeclaringType, LevelWarn, message, exception);
        }

        public void Warning(string format, params object[] arguments)
        {
            if (IsWarnEnabled)
                Logger.Log(ThisDeclaringType, LevelWarn, new SystemStringFormat(CultureInfo.InvariantCulture, format, arguments), null);
        }

        public void WarnFormat(string format, object arg0)
        {
            if (IsWarnEnabled)
                Logger.Log(ThisDeclaringType, LevelWarn, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0), null);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            if (IsWarnEnabled)
                Logger.Log(ThisDeclaringType, LevelWarn, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0, arg1), null);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            if (IsWarnEnabled)
                Logger.Log(ThisDeclaringType, LevelWarn, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0, arg1, arg2), null);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] arguments)
        {
            if (IsWarnEnabled)
                Logger.Log(ThisDeclaringType, LevelWarn, new SystemStringFormat(provider, format, arguments), null);
        }

        public void Error(string message)
        {
            Logger.Log(ThisDeclaringType, _levelError, message, null);
        }

        public void Error(string message, Exception exception)
        {
            Logger.Log(ThisDeclaringType, _levelError, message, exception);
        }

        public void Error(Exception exception)
        {
            Error(exception.Message, exception);
        }

        public void Error(string format, params object[] arguments)
        {
            if (IsErrorEnabled)
                Logger.Log(ThisDeclaringType, _levelError, new SystemStringFormat(CultureInfo.InvariantCulture, format, arguments), null);
        }

        public void Error(string format, object arg0)
        {
            if (IsErrorEnabled)
                Logger.Log(ThisDeclaringType, _levelError, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0), null);
        }

        public void Error(string format, object arg0, object arg1)
        {
            if (IsErrorEnabled)
                Logger.Log(ThisDeclaringType, _levelError, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0, arg1), null);
        }

        public void Error(string format, object arg0, object arg1, object arg2)
        {
            if (IsErrorEnabled)
                Logger.Log(ThisDeclaringType, _levelError, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0, arg1, arg2), null);
        }

        public void Exception(IFormatProvider provider, string format, params object[] arguments)
        {
            if (IsErrorEnabled)
                Logger.Log(ThisDeclaringType, _levelError, new SystemStringFormat(provider, format, arguments), null);
        }

        public void Fatal(string text)
        {
            Logger.Log(ThisDeclaringType, _levelFatal, text, null);
        }

        public void Fatal(string text, Exception exception)
        {
            Logger.Log(ThisDeclaringType, _levelFatal, text, exception);
        }

        public void Fatal(Exception exception)
        {
            Fatal(exception.Message, exception);
        }

        public void AddMessage(Severity severity, string formatString, params object[] values)
        {
            switch (severity)
            {
                case Severity.Debug:
                    log.DebugFormat(formatString, values);
                    break;
                case Severity.Info:
                    log.InfoFormat(formatString, values);
                    break;
                case Severity.Warning:
                    log.WarnFormat(formatString, values);
                    break;
                case Severity.Error:
                    log.ErrorFormat(formatString, values);
                    break;
                case Severity.FatalError:
                    log.FatalFormat(formatString, values);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("severity");
            }
        }

        public void FatalFormat(string format, params object[] arguments)
        {
            if (IsFatalEnabled)
                Logger.Log(ThisDeclaringType, _levelFatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, arguments), null);
        }

        public void FatalFormat(string format, object arg0)
        {
            if (IsFatalEnabled)
                Logger.Log(ThisDeclaringType, _levelFatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0), null);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            if (IsFatalEnabled)
                Logger.Log(ThisDeclaringType, _levelFatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0, arg1), null);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            if (IsFatalEnabled)
                Logger.Log(ThisDeclaringType, _levelFatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg0, arg1, arg2), null);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] arguments)
        {
            if (IsFatalEnabled)
                Logger.Log(ThisDeclaringType, _levelFatal, new SystemStringFormat(provider, format, arguments), null);
        }

        private void ReloadLevels(ILoggerRepository repository)
        {
            var levelMap = repository.LevelMap;
            _levelDebug = levelMap.LookupWithDefault(Level.Debug);
            _levelInfo = levelMap.LookupWithDefault(Level.Info);
            LevelWarn = levelMap.LookupWithDefault(Level.Warn);
            _levelError = levelMap.LookupWithDefault(Level.Error);
            _levelFatal = levelMap.LookupWithDefault(Level.Fatal);
            _levelTrace = levelMap.LookupWithDefault(Level.Trace);
        }

        private void LoggerRepositoryConfigurationChanged(object sender, EventArgs e)
        {
            var repository = sender as ILoggerRepository;
            if (repository != null)
                ReloadLevels(repository);
        }

    }
}