using log4net;
using log4net.Appender;
using log4net.Core;
using Savchin.Services.Execution;

namespace Savchin.Services.Logging
{
    public interface ILoggerService: IRunnable
    {
        Level RootLoggingLevel { get; set; }

        /// <summary>
        /// Creates the appender.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <param name="appenderName">Name of the appender.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        IAppender CreateAppender(string loggerName, string appenderName, string fileName);

        /// <summary>
        /// Adds the appenders.
        /// </summary>
        /// <param name="args">The arguments.</param>
        void AddAppenders(params IAppender[] args);

        /// <summary>
        /// Removes the appenders.
        /// </summary>
        /// <param name="args">The arguments.</param>
        void RemoveAppenders(params IAppender[] args);

        /// <summary>
        /// Gets the appender.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IAppender GetAppender(string name);


        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        ILog GetLogger(string name);
    }
}