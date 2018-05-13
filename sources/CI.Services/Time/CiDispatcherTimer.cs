using System;
using System.Windows.Threading;

namespace Savchin.Services.Time
{
    public class CiDispatcherTimer : IDispatcherTimer
    {
        #region Fields

        private readonly DispatcherTimer _timer;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        public TimeSpan Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled
        {
            get { return _timer.IsEnabled; }
            set { _timer.IsEnabled = value; }
        }

        /// <summary>
        /// Occurs when [tick].
        /// </summary>
        public event EventHandler Tick
        {
            add { _timer.Tick += value; }
            remove { _timer.Tick -= value; }
        }

        #endregion Properties

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CiDispatcherTimer" /> class.
        /// </summary>
        public CiDispatcherTimer()
        {
            _timer = new DispatcherTimer();
        }

        #endregion Construction

        #region Public members

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _timer.Start();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }

        #endregion Public members
    }
}