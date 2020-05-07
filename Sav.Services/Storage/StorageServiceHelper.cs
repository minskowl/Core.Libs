using Savchin.Xml;

namespace Savchin.Services.Storage
{
    public static class StorageServiceHelper
    {
        /// <summary>
        /// Loads the specified storage service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storageService">The storage service.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        /// <returns></returns>
        public static T Load<T>(this IStorageService storageService, string fileName, FolderType folderType)
        {
            return storageService.LoadText(fileName, folderType).ToObject<T>();
        }

        /// <summary>
        /// Saves the specified storage service.
        /// </summary>
        /// <param name="storageService">The storage service.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        /// <param name="data">The data.</param>
        public static void Save<T>(this IStorageService storageService, string fileName, FolderType folderType, T data)
        {
            storageService.SaveText(fileName, folderType, data.ToXml());
        }
    }
}
