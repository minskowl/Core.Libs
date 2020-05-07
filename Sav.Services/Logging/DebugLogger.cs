using Savchin.Logging;

namespace Savchin.Services.Logging
{

    public interface IDebugLogger : ILogger
    {
    }

    public class DebugLogger : LoggerBase, IDebugLogger
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogger"/> class.
        /// </summary>
        public DebugLogger()
            : base(typeof(DebugLogger))
        {
        }
    
    }
}