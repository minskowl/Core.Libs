using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Prism.Events;
using Savchin.ComponentModel;

namespace Savchin.Services.Localization
{
    /// <summary>
    /// Implements storage for all available cultures
    /// </summary>
    public class LocalizationRegistry : ObjectBase, ILocalizationRegistry
    {
        #region Properties

        private readonly IList<ILocalizationProvider> _providers;

        // ReSharper disable MemberCanBePrivate.Global

        private ILocalizationProvider _currentProvider;

        /// <summary>
        /// Gets or sets the current provider.
        /// </summary>
        /// <value>The current provider.</value>
        public ILocalizationProvider CurrentProvider
        {
            get { return _currentProvider; }
            set
            {
                if (_currentProvider == value) return;

                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                LocalizationHelper.CurrentProvider = _currentProvider = value;
                FlowDirection = value.FlowDirection;
                OnPropertyChanged(nameof(CurrentProvider), nameof(Resources), nameof(FlowDirection));
            }
        }
        
        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <value>The resources.</value>
        public object Resources => CurrentProvider.Resources;
        
        /// <summary>
        /// Gets the flow direction.
        /// </summary>
        public FlowDirection FlowDirection { get; private set; }

        // ReSharper restore MemberCanBePrivate.Global

        #endregion Properties

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationRegistry" /> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="localizationLoader">The localization loader.</param>
        /// <exception cref="System.ArgumentNullException">eventAggregator
        /// or
        /// logger</exception>
        // ReSharper disable once MemberCanBeProtected.Global
        public LocalizationRegistry(IEventAggregator eventAggregator, ILocalizationLoader localizationLoader)
        {
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            if (localizationLoader == null) throw new ArgumentNullException(nameof(localizationLoader));

            eventAggregator.GetEvent<LocalizationChangingEvent>().Subscribe(OnLocalizationChangingEvent);
            _providers = localizationLoader.SelectProviders().ToList();
            CurrentProvider = _providers.FirstOrDefault();
        }

        #endregion Construction

        #region Event handlers

        private void OnLocalizationChangingEvent(LocalizationChangingEventArgs args)
        {
            if (args != null)
                CurrentProvider = _providers.FirstOrDefault(e => e.Language == args.LanguageInfo.Language);
        }

        #endregion Event handlers
    }
}
