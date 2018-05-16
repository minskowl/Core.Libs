using Savchin.IO;

namespace Savchin.Interfaces
{
    /// <summary>
    /// Class provide to access to FileSystem.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Gets the file manager.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        IFileManager File { get; }

        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        IDirectoryManager Directory { get; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        IPathManager Path { get; }
    }

    public interface IFileInfo
{
    /// <summary>
    /// Gets the name of the directory.
    /// </summary>
    /// <value>
    /// The name of the directory.
    /// </value>
    string DirectoryName { get; }

    /// <summary>
    /// Gets the full name.
    /// </summary>
    /// <value>
    /// The full name.
    /// </value>
    string FullName { get; }

    /// <summary>
    /// Gets the size.
    /// </summary>
    /// <value>
    /// The size.
    /// </value>
    StorageSize Size { get; }
}

}