using System;
using Prism.Events;
using Savchin.Logging;
using Savchin.Services.Execution;

namespace Savchin.Services.Time
{
    /// <summary>
    /// Reaises timer events every second
    /// </summary>
    public class TimeService : TimerRunnableBase, ITimeService
    {
        #region Constants

        private const int TimerIntervalMiliseconds = 1000;

        #endregion Constants

        #region Fields

        private readonly LocalTimeChangedEvent _event;

        #endregion Fields

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeService"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="logger">The logger.</param>
        public TimeService(IEventAggregator eventAggregator, ILogger logger)
            : base(logger, TimerIntervalMiliseconds)
        {
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

            _event = eventAggregator.GetEvent<LocalTimeChangedEvent>();
        }

        #endregion Construction

        #region Implementation of ITimeService

        /// <summary>
        /// Gets the local.
        /// </summary>
        /// <value>
        /// The local.
        /// </value>
        public TimeZoneInfo Local 
        {
            get { return TimeZoneInfo.Local; }
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public TimeZoneInfo GetTimeZone(string id)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the now.
        /// </summary>
        /// <value></value>
        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Gets the UTC now.
        /// </summary>
        /// <value></value>
        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

        #endregion  Implementation of ITimeService

        #region Overrides of TimerRunnableBase

        /// <summary>
        /// Called when [timer elapsed].
        /// </summary>
        protected override void OnTimerElapsed()
        {
            _event.Publish(DateTime.Now);
        }

        #endregion Overrides of TimerRunnableBase
    }
}
