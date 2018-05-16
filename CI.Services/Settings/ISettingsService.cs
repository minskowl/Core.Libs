using System;

namespace Savchin.Services.Settings
{
    /// <summary>
    /// Defines interface for service which is working with application settings
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Saves the specified data type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="data">The data.</param>
        void SaveData<T>(Enum dataType, T data);

        /// <summary>
        /// Loads the specified data type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataType">Type of the data.</param>
        /// <returns></returns>
        T LoadData<T>(Enum dataType);
        
        /// <summary>
        /// Saves the text.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="text">The text.</param>
        void SaveText(Enum dataType, string text);

        /// <summary>
        /// Loads the text.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns></returns>
        string LoadText(Enum dataType);
    }
}
