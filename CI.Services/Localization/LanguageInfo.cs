using System.Globalization;
using Savchin.Core;

namespace Savchin.Services.Localization
{
    public class LanguageInfo
    {
        public string LanguageName { get; }
        public CultureInfo Culture { get; }
        public Language Language { get; private set; }
        public int CultureId { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageInfo"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        public LanguageInfo(Language language)
        {
            Language = language;
            CultureId = (int)language;
            Culture = new CultureInfo(language.GetDescription());
            if (Culture.LCID == 1)
            {
                Culture.NumberFormat.DigitSubstitution = DigitShapes.None;
                Culture.NumberFormat.CurrencyNegativePattern = 1;
                Culture.NumberFormat.NumberNegativePattern = 1;
                Culture.NumberFormat.PercentNegativePattern = 0;
            }

            if (Language == Language.German)
                Culture.NumberFormat.NumberDecimalSeparator = ",";

            LanguageName = Culture.GetShortNativeName();
        }

        public override string ToString()
        {
            return LanguageName;
        }
    }
}