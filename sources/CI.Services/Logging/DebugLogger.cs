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
            : base("Debug")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogger"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DebugLogger(string name)
            : base(name)
        {
        }
    }
}