
namespace Savchin.Services.Localization
{

    /// <summary>
    /// 
    /// </summary>
    public interface ILocalizationService
    {
        LanguageInfo[] Languages { get; }

        /// <summary>
        /// Gets the current language.
        /// </summary>
        /// <value>
        /// The current language.
        /// </value>
        LanguageInfo CurrentLanguage { get; }


        void ChangeCurrentLanguage(LanguageInfo language);
    }
}