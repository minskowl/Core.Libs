namespace Savchin.Services.Storage
{
    /// <summary>
    /// Defines interface for instance which can build paths to application folders
    /// </summary>
    public interface IPathBuilder
    {
        /// <summary>
        /// Gets the folder path.
        /// </summary>
        /// <param name="folderType">Type of the folder.</param>
        string GetFolderPath(FolderType folderType);
    }
}
