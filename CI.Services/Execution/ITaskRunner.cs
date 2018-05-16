using System;
using System.Threading;
using System.Threading.Tasks;

namespace Savchin.Services.Execution
{
    public interface ITaskRunner
    {
        /// <summary>
        /// Runs the task.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        Task RunTask(Action action);

        /// <summary>
        /// Runs the task.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task RunTask(Action action, CancellationToken token );

        /// <summary>
        /// Whens all.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="tasks">The tasks.</param>
        /// <returns></returns>
        Task WhenAll(CancellationToken token, params Task[] tasks);
    }
}