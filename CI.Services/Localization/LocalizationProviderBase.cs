using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows;
using JetBrains.Annotations;
using Savchin.ComponentModel;
using Savchin.Core;

namespace Savchin.Services.Localization
{
    /// <summary>
    /// LocalizationProviderBase
    /// </summary>
    public abstract class LocalizationProviderBase : ILocalizationProvider
    {
        #region Properties

        /// <summary>
        /// Returns instance localization manager
        /// </summary>
        public object Resources { get; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        public CultureInfo Culture { get; }

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public Language Language { get; }

        /// <summary>
        /// Gets the culture identifier.
        /// </summary>
        /// <value>
        /// The culture identifier.
        /// </value>
        public int CultureId { get; }

        /// <summary>
        /// Gets the flow direction.
        /// </summary>
        /// <value>
        /// The flow direction.
        /// </value>
        public FlowDirection FlowDirection { get; }

        /// <summary>
        /// Gets the name of the language.
        /// </summary>
        /// <value>
        /// The name of the language.
        /// </value>
        public string LanguageName { get; }

        #endregion Properties

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationProviderBase"/> class.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="resources">The resources.</param>
        /// <exception cref="System.ArgumentNullException">
        /// culture
        /// or
        /// resources
        /// </exception>
        protected LocalizationProviderBase(int cultureId, [NotNull] CultureInfo culture, object resources)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));
            if (resources == null) throw new ArgumentNullException(nameof(resources));

            Language = (Language)cultureId;
            Culture = culture;
            LanguageName = Culture.GetShortNativeName();
            Resources = resources;
            CultureId = cultureId;
            FlowDirection = culture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        #endregion Construction

        #region Implementation of ILocalizationProvider

        /// <summary>
        /// Gets the localized string by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="returnNull">if set to <c>true</c> [return null].</param>
        /// <returns>
        /// Returns localized value for specified id
        /// </returns>
        public abstract string GetString(string id, bool returnNull = false);

        /// <summary>
        /// Gets the LTR marked string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string GetLtrMarkedString(string text)
        {
            return Language == Language.Arabic ? $"\u200E{text}\u200E" : text;
        }

        /// <summary>
        /// Gets or sets the string format.
        /// </summary>
        /// <value>
        /// The string format.
        /// </value>
        public StringFormat StringFormat { get; protected set; }

        /// <summary>
        /// Gets the localized string for enum value.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public string GetString(Enum id)
        {
            return GetString($"{id.GetType().Name}_{id}");
        }

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ItemModel<T>[] GetModels<T>()
        {
            var type = typeof(T);
            var typeName = type.Name;
            return EnumHelper.GetValues<T>().Select(e => new ItemModel<T>(GetString($"{typeName}_{e.ToString()}"), e)).ToArray();
        }

        /// <summary>
        /// Gets the translation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public NameValuePair<T>[] GetTranslation<T>()
        {
            var values = EnumHelper.GetValuesArray<T>();

            var typeName = typeof(T).Name;
            return values.Select(e => new NameValuePair<T>(GetString($"{typeName}_{e.ToString()}"), e)).ToArray();
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public ItemModel<T>[] GetModels<T>(IEnumerable<T> items)
        {
            var typeName = typeof(T).Name;
            return items.Select(e => new ItemModel<T>(GetString($"{typeName}_{e.ToString()}"), e)).ToArray();
        }

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public ItemModel<Enum>[] GetModels(Type type)
        {
            var typeName = type.Name;
            return EnumHelper.GetValues(type).Select(e => new ItemModel<Enum>(GetString($"{typeName}_{e.ToString()}"), e)).ToArray();
        }

        /// <summary>
        /// Gets the translation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        public NameValuePair<T>[] GetTranslation<T>(string prefix)
        {
            var values = EnumHelper.GetValuesArray<T>();

            var typeName = typeof(T).Name;
            return values.Select(e => new NameValuePair<T>(GetString($"{prefix}_{typeName}_{e.ToString()}"), e)).ToArray();
        }

        /// <summary>
        /// Gets the translation.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public NameValuePair[] GetTranslation(Type type)
        {
            var values = EnumHelper.GetValuesArray(type);
            return values.Select(e => new NameValuePair(GetString($"{type.Name}_{e}"), e)).ToArray();
        }

        #endregion Implementation of ILocalizationProvider
    }
}
