using System;
using Savchin.Logging;

namespace Savchin.Services.Core
{
    public class ServiceBase
    {
        protected ILogger Logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ServiceBase(ILogger logger)
        {
            Logger = logger;
        }


        /// <summary>
        /// Called when [error].
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected virtual void OnError(Exception ex)
        {
            Logger.Warning(ex.ToString());
        }
    }
}
