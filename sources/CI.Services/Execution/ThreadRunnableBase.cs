using System;
using System.Threading;

namespace Savchin.Services.Execution
{
    /// <summary>
    /// Defines base class for thread based runnable processors
    /// </summary>
    public abstract class ThreadRunnableBase : IRunnable
    {
        #region Fields

        private readonly string _threadName;
        private Thread _processingThread;
        protected bool IsStopping { get; private set; }


        #endregion

        #region Properties

        /// <summary>
        /// Gets the sleep interval.
        /// </summary>
        /// <value>The sleep interval.</value>
        protected abstract int SleepInterval
        {
            get;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadRunnableBase"/> class.
        /// </summary>
        /// <param name="threadName">Name of the thread.</param>
        protected ThreadRunnableBase(string threadName)
        {
            _threadName = threadName;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_processingThread != null)
                Stop();
        }

        #endregion

        #region Implementation of IRunnable

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (_processingThread != null)
                throw new InvalidOperationException("Request processor can't be started twice");

            _processingThread = new Thread(ThreadProcedure)
                                    {
                                        IsBackground = true,
                                        Name = _threadName
                                    };
            _processingThread.Start();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (_processingThread == null)
                throw new InvalidOperationException("Request processor can't stopped without starting");

            if (IsStopping)
                return;

            IsStopping = true;

            if (!_processingThread.Join(new TimeSpan(0, 0, 10)))
                _processingThread.Abort();

            _processingThread = null;
            IsStopping = false;
        }

        #endregion

        #region Virtual methods

        /// <summary>
        /// This method is executed in separate thread
        /// </summary>
        protected abstract void Process();

        #endregion

        #region helper methods

        private void ThreadProcedure()
        {
            do
            {
                Process();

                Thread.Sleep(SleepInterval);
            }
            while (!IsStopping);

        }

        #endregion
    }
}