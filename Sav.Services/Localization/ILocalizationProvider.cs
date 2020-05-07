using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows;
using Savchin.ComponentModel;
using Savchin.Core;

namespace Savchin.Services.Localization
{
    /// <summary>
    /// Defines interface for localization provider
    /// </summary>
    public interface ILocalizationProvider
    {
        /// <summary>
        /// Returns instance localization manager
        /// </summary>
        object Resources { get; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <value>The culture.</value>
        CultureInfo Culture { get; }

        /// <summary>
        /// Gets the name of the language.
        /// </summary>
        /// <value>The name of the language.</value>
        string LanguageName { get; }

        /// <summary>
        /// Gets the string format.
        /// </summary>
        /// <value>
        /// The string format.
        /// </value>
        StringFormat StringFormat { get; }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        string GetString(Enum id);

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="returnNull">if set to <c>true</c> [return null].</param>
        /// <returns>
        /// Returns localized value for specified id
        /// </returns>
        string GetString(string id, bool returnNull=false);

        /// <summary>
        /// Gets the translation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        NameValuePair<T>[] GetTranslation<T>();

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        ItemModel<T>[] GetModels<T>(IEnumerable<T> items);

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        ItemModel<Enum>[] GetModels(Type type);

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ItemModel<T>[] GetModels<T>();

        /// <summary>
        /// Gets the translation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        NameValuePair<T>[] GetTranslation<T>(string prefix);
        /// <summary>
        /// Gets the translation.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        NameValuePair[] GetTranslation(Type type);

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        Language Language { get; }

        /// <summary>
        /// Gets the culture identifier.
        /// </summary>
        /// <value>
        /// The culture identifier.
        /// </value>
        int CultureId { get; }

        ///// <summary>
        ///// Gets the flow direction.
        ///// </summary>
        ///// <value>
        ///// The flow direction.
        ///// </value>
        //FlowDirection FlowDirection { get; }

        /// <summary>
        /// Gets the LTR marked string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ltr")]
        string GetLtrMarkedString(string text);
    }
}
