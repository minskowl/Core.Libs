
namespace Savchin.Services.Localization
{
    /// <summary>
    /// Defines localization registry functionality
    /// </summary>
    public interface ILocalizationRegistry
    {


        /// <summary>
        /// Gets or sets the current provider.
        /// </summary>
        /// <value>The current provider.</value>
        ILocalizationProvider CurrentProvider
        {
            get;
            set;
        }

        
    }
}
