using System;
using System.Collections.Generic;
using System.Threading;

namespace Savchin.Services.Execution
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public sealed class TaskQueue : ITaskQueue, IDisposable
    {
        #region Fields

        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        private readonly object _syncRoot = new object();
        private readonly Queue<IRequest> _queue = new Queue<IRequest>();
        private bool _disposed;

        #endregion Fields

        #region ITaskQueue implementation

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (_syncRoot)
                _queue.Clear();
        }

        /// <summary>
        /// Adds the specified request to the end of queue
        /// </summary>
        /// <param name="request">The request to add</param>
        /// <param name="priority">The priority.</param>
        public void Enqueue(IRequest request, RequestPriority priority = RequestPriority.Normal)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            bool wait;
            lock (_syncRoot)
            {
                wait = _queue.Count == 0;
                _queue.Enqueue(request);
            }
            if (wait)
                _autoResetEvent.Set();
        }

        /// <summary>
        /// Removes request from queue and returns to caller
        /// </summary>
        /// <returns>Request instance</returns>
        public IRequest Dequeue()
        {
            if (_queue.Count == 0)
                _autoResetEvent.WaitOne();
            lock (_syncRoot)
            {
                return _queue.Count == 0 ? null : _queue.Dequeue();
            }
        }

        /// <summary>
        /// Wakeups this instance.
        /// </summary>
        public void Wakeup()
        {
            _autoResetEvent.Set();
        }

        #endregion ITaskQueue implementation

        #region IDisposable implementation

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _autoResetEvent.Close();
            }
            _disposed = true;
        }

        ~TaskQueue()
        {
            Dispose(false);
        }

        #endregion IDisposable implementation
    }
}
