//using System;
//using System.Linq;
//using log4net;
//using log4net.Appender;
//using log4net.Core;
//using log4net.Filter;
//using log4net.Layout;
//using log4net.Repository.Hierarchy;
//using Savchin.Collection.Generic;

//namespace Savchin.Services.Logging
//{
//    public class LoggerService : ILoggerService
//    {
//        #region Data

//        private readonly Hierarchy _repository;

//        /// <summary>
//        /// Gets or sets the root logging level.
//        /// </summary>
//        /// <value>
//        /// The root logging level.
//        /// </value>
//        public Level RootLoggingLevel
//        {
//            get { return _repository.Root.Level; }
//            set
//            {
//                _repository.Root.Level = value;
//                RaiseConfigurationChanged();
//            }
//        }

//        #endregion

//        #region Construction

//        /// <summary>
//        /// Initializes a new instance of the <see cref="LoggerService"/> class.
//        /// </summary>
//        public LoggerService()
//        {
//            _repository = (Hierarchy) LogManager.GetRepository();
//        }

//        #endregion

//        #region Implementation of ILoggerService

//        /// <summary>
//        /// Creates the appender.
//        /// </summary>
//        /// <param name="loggerName">Name of the logger.</param>
//        /// <param name="appenderName">Name of the appender.</param>
//        /// <param name="fileName">Name of the file.</param>
//        /// <returns></returns>
//        public IAppender CreateAppender(string loggerName, string appenderName, string fileName)
//        {
//            var layout = new PatternLayout
//            {
//                ConversionPattern = "%newline===%-5p==%d %newlinemessage: %m%newline exception: %exception"
//            };
//            layout.ActivateOptions();
//            var traceAppender = new FileAppender
//            {
//                Name = appenderName,
//                File = fileName,
//                Layout = layout,
//                LockingModel = new FileAppender.MinimalLock()
//            };

//            traceAppender.AddFilter(new LoggerMatchFilter
//            {
//                LoggerToMatch = loggerName,
//                Next = new DenyAllFilter()
//            });
//            traceAppender.ActivateOptions();
//            return traceAppender;
//        }

//        /// <summary>
//        /// Adds the appenders.
//        /// </summary>
//        /// <param name="args">The arguments.</param>
//        public void AddAppenders(params IAppender[] args)
//        {
//            if (args.IsEmpty()) return;

//            args.ForEach(_repository.Root.AddAppender);

//            RaiseConfigurationChanged();
//        }

//        /// <summary>
//        /// Removes the appenders.
//        /// </summary>
//        /// <param name="args">The arguments.</param>
//        public void RemoveAppenders(params IAppender[] args)
//        {
//            if (args.IsEmpty()) return;

//            args.ForEach(a =>
//            {
//                _repository.Root.RemoveAppender(a);
//                a.Close();
//            });

//            RaiseConfigurationChanged();
//        }

//        /// <summary>
//        /// Gets the appender.
//        /// </summary>
//        /// <param name="name">The name.</param>
//        /// <returns></returns>
//        public  IAppender GetAppender(string name)
//        {
//            return _repository.GetAppenders().FirstOrDefault(a => a.Name == name);
//        }

//        /// <summary>
//        /// Gets the logger.
//        /// </summary>
//        /// <param name="name">The name.</param>
//        /// <returns></returns>
//        public ILog GetLogger(string name)
//        {
//            return LogManager.GetCurrentLoggers().FirstOrDefault(x=>x.Logger.Name == name);
//        }

//        #endregion

//        #region Helper methods

//        private void RaiseConfigurationChanged()
//        {
//            _repository.Configured = true;
//            _repository.RaiseConfigurationChanged(EventArgs.Empty);
//        }

//        #endregion

//        public void Start()
//        {
//            LogManager.GetCurrentLoggers().ForEach(e => e.Info(new InitializeMarker()));
            
//        }

//        public void Stop()
//        {
//            LogManager.GetCurrentLoggers().ForEach(e => e.Info(new FinishMarker()));
//        }
//    }
//}