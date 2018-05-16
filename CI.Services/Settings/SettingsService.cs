using System;
using Savchin.Services.Storage;
using Savchin.Xml;

namespace Savchin.Services.Settings
{
    /// <summary>
    /// Implements settings storage logic
    /// </summary>
    public class SettingsService : ISettingsService
    {

        protected const FolderType Folder = FolderType.Settings;
        protected IStorageService StorageService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsService"/> class.
        /// </summary>
        /// <param name="storageService">The storage service.</param>
        public SettingsService(IStorageService storageService)
        {
            if (storageService == null)
                throw new ArgumentNullException("storageService");

            StorageService = storageService;
        }
        
        #region Implementation of ISettingsService

        /// <summary>
        /// Saves the specified data type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="data">The data.</param>
        public void SaveData<T>(Enum dataType, T data)
        {
            SaveText(dataType, data.ToXml());
        }

        /// <summary>
        /// Loads the specified data type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataType">Type of the data.</param>
        /// <returns></returns>
        public T LoadData<T>(Enum dataType)
        {
            return LoadText(dataType).ToObject<T>();
        }

        /// <summary>
        /// Saves the text.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="text">The text.</param>
        public virtual void SaveText(Enum dataType, string text)
        {
            StorageService.SaveText(dataType.ToString(), Folder, text);
        }

        /// <summary>
        /// Loads the text.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns></returns>
        public virtual string LoadText(Enum dataType)
        {
            return StorageService.LoadText(dataType.ToString(), Folder);
        }

        #endregion Implementation of ISettingsService
    }
}
