using System;
using Savchin.Logging;

namespace Savchin.Services.Logging
{
    public class NullLogger : ILogger
    {
        /// <summary>
        /// Gets a value indicating whether this instance is debug enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is debug enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsTraceEnabled => false;

        /// <summary>
        /// Gets a value indicating whether this instance is information enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is information enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsInfoEnabled => false;

        /// <summary>
        /// Gets a value indicating whether this instance is warning enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is warning enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsWarningEnabled => false;

        /// <summary>
        /// Gets a value indicating whether this instance is debug enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is debug enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDebugEnabled => false;

        /// <summary>
        /// Traces the specified format.
        /// </summary>
        /// <param name="message">The format.</param>
        public void Trace(string message)
        {
        }

        /// <summary>
        /// Exceptions the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(string message, Exception exception)
        {
        }

        /// <summary>
        /// Errors the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Error(Exception exception)
        {
        }

        /// <summary>
        /// Exceptions the specified format string.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arguments">The arguments.</param>
        public void Error(string format, params object[] arguments)
        {
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
        }

        /// <summary>
        /// Informations the specified format string.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arguments">The arguments.</param>
        public void Info(string format, params object[] arguments)
        {
        }

        /// <summary>
        /// Informations the specified format.
        /// </summary>
        /// <param name="message">The format.</param>
        /// <param name="exception">The exception.</param>
        public void Info(string message, Exception exception)
        {
        }

        /// <summary>
        /// Warnings the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Warning(Exception exception)
        {
        }

        /// <summary>
        /// Warnings the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Warning(string text)
        {
        }

        /// <summary>
        /// Warnings the specified format string.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arguments">The arguments.</param>
        public void Warning(string format, params object[] arguments)
        {
        }

        /// <summary>
        /// Warnings the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Warning(string message, Exception exception)
        {
        }

        /// <summary>
        /// Debugs the specified format string.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arguments">The arguments.</param>
        public void Debug(string format, params object[] arguments)
        {
        }

        /// <summary>
        /// Debugs the specified object.
        /// </summary>
        /// <param name="value">The object.</param>
        public void Debug(object value)
        {
        }

        /// <summary>
        /// Traces the specified format string.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arguments">The arguments.</param>
        public void Trace(string format, params object[] arguments)
        {
        }
        
        /// <summary>
        /// Logs fatal level text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Fatal(string text)
        {
        }

        /// <summary>
        /// Logs fatal level text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="exception">The exception.</param>
        public void Fatal(string text, Exception exception)
        {
        }

        /// <summary>
        /// Logs fatal level exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Fatal(Exception exception)
        {
        }

        public void AddMessage(Severity severity, string formatString, params object[] values)
        {
            
        }
    }
}
