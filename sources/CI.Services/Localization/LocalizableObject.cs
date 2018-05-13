using System;
using System.Collections.Generic;
using System.ComponentModel;
using Savchin.ComponentModel;
using Savchin.Core;

namespace Savchin.Services.Localization
{
    /// <summary>
    /// LocalizableObject
    /// </summary>
    public class LocalizableObject : ObjectBase, ILocalizationDepended
    {
        protected ILocalizationRegistry LocalizationRegistry { get; }

        [Browsable(false)]
        protected ILocalizationProvider Localization => LocalizationRegistry?.CurrentProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableObject" /> class.
        /// </summary>
        /// <param name="localizationRegistry">The localization registry.</param>
        protected LocalizableObject(ILocalizationRegistry localizationRegistry = null)
        {
            LocalizationRegistry = localizationRegistry;
        }

        void ILocalizationDepended.OnLocalizationChangedEvent(EventArgs obj)
        {
            OnLocalizationChangedEvent(obj);
        }

        /// <summary>
        /// Raises the <see cref="E:LocalizationChangedEvent"/> event.
        /// </summary>
        /// <param name="obj">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnLocalizationChangedEvent(EventArgs obj)
        {
            ForEachChild<ILocalizationDepended>(e => e.OnLocalizationChangedEvent(obj));
        }

        #region Localization
        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        protected string GetString(Enum id)
        {
            return Localization.GetString(id);
        }
        /// <summary>
        /// Gets the translation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected NameValuePair<T>[] GetTranslation<T>()
        {
            return Localization.GetTranslation<T>();
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="returnNull">if set to <c>true</c> [return null].</param>
        /// <returns></returns>
        protected string GetString(string key, bool returnNull = false)
        {
            return Localization.GetString(key, returnNull);
        }

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        protected ItemModel<T>[] GetModels<T>(IEnumerable<T> items)
        {
            return Localization.GetModels(items);
        }

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected ItemModel<Enum>[] GetModels<T>()
        {
            return Localization.GetModels(typeof(T));
        }

        /// <summary>
        /// Gets the typed models.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected ItemModel<T>[] GetTypedModels<T>()
        {
            return Localization.GetModels<T>();
        }
        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        protected ItemModel<T>[] GetModels<T>(params T[] items)
        {
            return Localization.GetModels(items);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The obj.</param>
        /// <returns></returns>
        protected string GetString(string key, object value)
        {
            return string.Format(Localization.GetString(key), value);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value1">The obj1.</param>
        /// <param name="value2">The obj2.</param>
        /// <returns></returns>
        protected string GetString(string key, object value1, object value2)
        {
            return string.Format(Localization.GetString(key), value1, value2);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="arguments">The args.</param>
        /// <returns></returns>
        protected string GetString(string key, params object[] arguments)
        {
            return string.Format(Localization.GetString(key), arguments);
        }


        #endregion
    }
}
