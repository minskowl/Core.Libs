using System.Net;
using Savchin.Services.Execution;
using Savchin.SystemEnvironment;

namespace Savchin.Services.SystemEnvironment
{
    public interface IEnvironmentService : IRunnable
    {
        /// <summary>
        /// Gets or sets the default proxy.
        /// </summary>
        /// <value>
        /// The default proxy.
        /// </value>
        IWebProxy DefaultProxy { get; set; }

        /// <summary>
        /// Gets the system web proxy.
        /// </summary>
        /// <value>
        /// The system web proxy.
        /// </value>
        IWebProxy SystemWebProxy { get; }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        EnvironmentInfo Environment { get; }

        /// <summary>
        /// Gets the hardware.
        /// </summary>
        /// <value>
        /// The hardware.
        /// </value>
        HardwareInfo Hardware { get; }

        /// <summary>
        /// Garbages the collect.
        /// </summary>
        void GarbageCollect();
    }
}