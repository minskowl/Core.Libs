using System;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Threading;
using Microsoft.Win32;
using Prism.Events;
using Savchin.Logging;
using Savchin.SystemEnvironment;

namespace Savchin.Services.SystemEnvironment
{
    public class EnvironmentService :  IEnvironmentService
    {
        #region Fields

        private readonly PropertyInfo _userDefaultCultureProperty;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger _logger;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the hardware.
        /// </summary>
        /// <value>
        /// The hardware.
        /// </value>
        public HardwareInfo Hardware => HardwareInfo.GetCurrent(_logger);

        /// <summary>
        /// Garbages the collect.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect")]
        public void GarbageCollect()
        {
            _logger.Trace("Start GC collecting");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
            GC.Collect();
            _logger.Trace("End GC collecting");
        }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        public EnvironmentInfo Environment => EnvironmentInfo.GetCurrent();
        
        /// <summary>
        /// Gets or sets the default proxy.
        /// </summary>
        /// <value>
        /// The default proxy.
        /// </value>
        public IWebProxy DefaultProxy
        {
            get { return WebRequest.DefaultWebProxy; }
            set { WebRequest.DefaultWebProxy = value; }
        }

        /// <summary>
        /// Gets the system web proxy.
        /// </summary>
        /// <value>
        /// The system web proxy.
        /// </value>
        public IWebProxy SystemWebProxy => WebRequest.GetSystemWebProxy();

        #endregion Properties

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentService"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="logger">The logger.</param>
        public EnvironmentService(IEventAggregator eventAggregator, ILogger logger)
        {
            _eventAggregator = eventAggregator;
            _logger = logger;
            _userDefaultCultureProperty = typeof(CultureInfo).GetProperty("UserDefaultCulture", BindingFlags.Static | BindingFlags.NonPublic);
        }

        #endregion Construction

        #region Public methods

        /// <summary>
        /// Starts this instance. Invokes in non UI threads
        /// </summary>
        public void Start()
        {
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
        }

        #endregion Public methods

        #region Event handlers

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category != UserPreferenceCategory.Locale)
                return;

            Thread.CurrentThread.CurrentCulture.ClearCachedData();

            var val = _userDefaultCultureProperty.GetValue(null, null) as CultureInfo;
            if (val != null)
                _eventAggregator.GetEvent<ChangeLocaleEvent>().Publish(val);
        }

        #endregion Event handlers
    }
}
