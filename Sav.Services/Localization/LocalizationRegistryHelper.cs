namespace Savchin.Services.Localization
{
    /// <summary>
    /// LocalizationRegistry Helper
    /// </summary>
    public static class LocalizationRegistryHelper
    {
        /// <summary>
        /// Gets the string from CurrentProvider.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetString(this ILocalizationRegistry registry, string key)
        {
            return registry.CurrentProvider.GetString(key);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <param name="key">The key.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string GetString(this ILocalizationRegistry registry, string key, params object[] args)
        {
            return string.Format( registry.CurrentProvider.GetString(key),args);
        }
    }
}
