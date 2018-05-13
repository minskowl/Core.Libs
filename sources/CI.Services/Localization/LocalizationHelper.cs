namespace Savchin.Services.Localization
{
   public static class LocalizationHelper
    {
        internal static ILocalizationProvider CurrentProvider { private get; set; }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static string GetString(string key,params object[] args)
        {
            return string.Format(CurrentProvider.GetString(key), args);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetString(string key, object value)
        {
            return string.Format(CurrentProvider.GetString(key), value);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            return CurrentProvider.GetString(key);
        }
    }
}
