using System;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Savchin.Services.Execution
{
    public class TaskRunner : ITaskRunner
    {
        /// <summary>
        /// Runs the task.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public Task RunTask(Action action)
        {
            return Task.Run(action);
        }

        /// <summary>
        /// Runs the task.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public Task RunTask(Action action, CancellationToken token)
        {
            return Task.Run(action, token);
        }

        /// <summary>
        /// Whens all.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="tasks">The tasks.</param>
        /// <returns></returns>
        public Task WhenAll(CancellationToken token, params Task[] tasks)
        {
            return Task.WhenAny(Task.WhenAll(tasks), token.AsTask());
        }
    }
}
