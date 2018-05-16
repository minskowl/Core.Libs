using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Prism.Events;
using Savchin.Collection.Generic;
using Savchin.Core;
using Savchin.Logging;
using Savchin.Services.Core;
using Savchin.Services.Execution;
using Savchin.Services.Settings;
using Savchin.Services.SystemEnvironment;

namespace Savchin.Services.Localization
{


    public class LocalizationService : RunnableServiceBase, ILocalizationService, IRunnable
    {
        private readonly DateTimeFormatInfo _systemFormat;
        private readonly ISettingsService _settingsService;
        private readonly Enum _settingsType;
        private readonly LocalizationChangedEvent _localizationChangedEvent;
        private readonly LocalizationChangingEvent _localizationChangingEvent;
        private readonly ITaskQueue _taskQueue;
        private readonly IDispatcher _dispatcher;
        /// <summary>
        /// Gets the languages.
        /// </summary>
        /// <value>
        /// The languages.
        /// </value>
        public LanguageInfo[] Languages { get; }

        private LanguageInfo _languageInfo;
        /// <summary>
        /// Gets the current language.
        /// </summary>
        /// <value>
        /// The current language.
        /// </value>
        public LanguageInfo CurrentLanguage
        {
            get { return _languageInfo; }
            private set { _languageInfo = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationService" /> class.
        /// </summary>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="taskQueue">The task queue.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public LocalizationService(ISettingsService settingsService, IEventAggregator eventAggregator, ILogger logger, ITaskQueue taskQueue, IDispatcher dispatcher)
            : base(logger)
        {
            _systemFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
            _settingsService = settingsService;
            _settingsType = SettingsDataType.Localization;

            _taskQueue = taskQueue;
            _dispatcher = dispatcher;

            _localizationChangedEvent = eventAggregator.GetEvent<LocalizationChangedEvent>();
            _localizationChangingEvent = eventAggregator.GetEvent<LocalizationChangingEvent>();

            Languages = EnumHelper.GetValues<Language>().OrderBy(x => x.GetOrder()).Select(e => new LanguageInfo(e)).ToArray();

            eventAggregator.GetEvent<ChangeLocaleEvent>().Subscribe(OnChangeLocaleEvent, ThreadOption.UIThread);

        }

        /// <summary>
        /// Starts this instance. 
        /// </summary>
        public override void Start()
        {
            base.Start();

            var cultureValue = _settingsService.LoadData<string>(_settingsType);
            var culture = GetProvider(cultureValue) ?? GetDefaultLanguage() ?? Languages.FirstOrDefault();

            _dispatcher.EasyBeginInvoke((Action<LanguageInfo>) ChangeCurrentLanguage, culture);

        }

        /// <summary>
        /// Changes the current language.
        /// </summary>
        /// <param name="language">The language.</param>
        public void ChangeCurrentLanguage(LanguageInfo language)
        {
            language.Culture.DateTimeFormat = _systemFormat;

            var localizationChangingArgs = new LocalizationChangingEventArgs(language);
            _localizationChangingEvent.Publish(localizationChangingArgs);

            if (localizationChangingArgs.WaitingFor.IsEmpty())
                SetCurrentLanguage(language);
            else
            {
                var waitTask = new WaitTask(Logger)
                {
                    OnError = errorDto => Logger.Warning("Exception during localization changing " + errorDto.Exception),
                    Handlers = localizationChangingArgs.WaitingFor.ToArray(),
                    OnResult = result => { SetCurrentLanguage(language); }
                };

                _taskQueue.Enqueue(waitTask);
            }
        }

        private void SetCurrentLanguage(LanguageInfo obj)
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = obj.Culture;
            CurrentLanguage = obj;

            _localizationChangedEvent.Publish(EventArgs.Empty);

            _settingsService.SaveData(_settingsType, obj.Culture.LCID.ToString());
            Logger.Info("Localization changed {0}", _languageInfo.Language);
        }
        
        /// <summary>
        /// Gets the default language.
        /// </summary>
        /// <returns></returns>
        protected virtual LanguageInfo GetDefaultLanguage()
        {
            return null;
        }

        protected LanguageInfo GetProvider(string id)
        {
            int cultureLcid;
            return int.TryParse(id, out cultureLcid) ? Languages.FirstOrDefault(item => item.Culture.LCID == cultureLcid) : null;
        }

        private void OnChangeLocaleEvent(CultureInfo obj)
        {
            var culture = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
            if (culture != null)
            {
                culture.DateTimeFormat = obj.DateTimeFormat;
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = culture;
            }
            _localizationChangedEvent.Publish(EventArgs.Empty);
        }
    }
}
