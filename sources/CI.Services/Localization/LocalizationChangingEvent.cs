using System.Collections.Generic;
using System.Threading;
using Savchin.Services.CiPrism;

namespace Savchin.Services.Localization
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class LocalizationChangingEventArgs
    {
        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public LanguageInfo LanguageInfo { get; private set; }

        /// <summary>
        /// Gets the waiting for.
        /// </summary>
        /// <value>
        /// The waiting for.
        /// </value>
        public IList<WaitHandle> WaitingFor { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationChangingEventArgs"/> class.
        /// </summary>
        /// <param name="languageInfo">The language information.</param>
        public LocalizationChangingEventArgs(LanguageInfo languageInfo)
        {
            LanguageInfo = languageInfo;
            WaitingFor = new List<WaitHandle>();
        }
    }

    public class LocalizationChangingEvent : CiPubSubEvent<LocalizationChangingEventArgs>
    {
    }
}
