using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace Savchin.Services.Localization
{
    class LocalizationProvider : LocalizationProviderBase
    {
        private readonly Dictionary<string, string> _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationProvider"/> class.
        /// </summary>
        /// <param name="resources">The resources.</param>
        /// <param name="translations">The translations.</param>
        public LocalizationProvider(object resources, Translations translations)
            : base(translations.Id, translations.Culture, resources)
        {
            _map = translations.Map;

            //ar-SA
            if (Culture.LCID == 1)
            {
                Culture.NumberFormat.DigitSubstitution = DigitShapes.None;
                Culture.NumberFormat.CurrencyNegativePattern = 1;
                Culture.NumberFormat.NumberNegativePattern = 1;
                Culture.NumberFormat.PercentNegativePattern = 0;

                StringFormat = new StringFormat();
                StringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                StringFormat.SetDigitSubstitution(Culture.LCID, StringDigitSubstitute.None);
            }
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="returnNull">if set to <c>true</c> [return null].</param>
        /// <returns>
        /// Returns localized value for specified id
        /// </returns>
        public override string GetString(string id, bool returnNull = false)
        {
            string res;
            if (string.IsNullOrEmpty(id)) return null;

            return _map.TryGetValue(id, out res) ? res :
                (returnNull ? null : $"!{id}!");
        }
        
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Culture.ThreeLetterWindowsLanguageName;
        }
    }
}
