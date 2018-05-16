using System;

namespace Savchin.Services.Execution
{
    /// <summary>
    /// Defines contract of request which can be executed asynchronously by processor
    /// </summary>
    public interface IRequest : IDisposable
    {
        /// <summary>
        /// Executes this instance.
        /// </summary>
        void Execute(IDispatcher dispatcher);

    }
}
