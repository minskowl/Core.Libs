using System.Collections.Generic;
using System.Globalization;

namespace Savchin.Services.Localization
{
    public sealed class Translations
    {
        /// <summary>
        /// Gets the map.
        /// </summary>
        /// <value>
        /// The map.
        /// </value>
        public Dictionary<string, string> Map { get; private set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        public CultureInfo Culture { get; private set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Translations"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        public Translations(CultureInfo culture)
        {
            Map = new Dictionary<string, string>();
            Culture = culture;
        }
    }
}