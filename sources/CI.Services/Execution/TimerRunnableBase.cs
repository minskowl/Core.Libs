using System;
using System.Threading;
using Savchin.Logging;

namespace Savchin.Services.Execution
{
    /// <summary>
    /// Defines timer bases runnable instance
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",Justification = "Stop method as Dispose for service")]
    public abstract class TimerRunnableBase : IRunnable
    {
        #region Fields

        private readonly ILogger _logger;

        private Timer _timer;
        private object _synObject=new object();



        protected long Interval
        {
            get;
            set;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerRunnableBase"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="interval">The interval.</param>
        protected TimerRunnableBase(ILogger logger, int interval)
        {
            if (logger == null) throw new ArgumentNullException("logger");

            Interval = interval;
            _logger = logger;
            _timer = new Timer(OnTimer);
        }


        /// <summary>
        /// Prevents a default instance of the <see cref="TimerRunnableBase"/> class from being created.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected TimerRunnableBase(ILogger logger)
            : this(logger, 1000)
        {

        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles the Elapsed event of the _timer control.
        /// </summary>
        /// <param name="state">The state.</param>
        private void OnTimer(object state)
        {
            try
            {
                OnTimerElapsed();
            }
            catch (Exception ex)
            {
                _logger.Error("Fail in TimerRunnableBase \n " + ex);
            }
            finally
            {
                ActivateTimer();
            }
        }

        #endregion

        #region Virtual methods

        /// <summary>
        /// Called when [timer elapsed].
        /// </summary>
        protected abstract void OnTimerElapsed();

        #endregion


        #region Implementation of IRunnable

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public virtual void Start()
        {
            ActivateTimer();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public virtual void Stop()
        {
            if (_timer == null) return;

            lock (_synObject)
            {
                if (_timer == null) return;
                _timer.Dispose();
                _timer = null;
            }

        }

        #endregion

        protected void ActivateTimer()
        {
            if (_timer == null) return;
            lock (_synObject)
            {
                if (_timer == null) return;
                try
                {
                    _timer.Change(Interval, Timeout.Infinite);
                }
                catch (ObjectDisposedException ex)
                {
                    _logger.Warning(ex.ToString());
                }
            }

        }
    }
}