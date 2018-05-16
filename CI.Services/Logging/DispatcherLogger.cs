using Savchin.Logging;

namespace Savchin.Services.Logging
{
    public interface IDispatcherLogger : ILogger
    {

    }

    public class DispatcherLogger : LoggerBase, IDispatcherLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppLogger"/> class.
        /// </summary>
        public DispatcherLogger()
            : base("Dispatcher")
        {
        }
        public DispatcherLogger(string name) : base(name)
        {
        }
    }
}