using System.IO;
using Savchin.Csv;

namespace Savchin.Services.Storage
{
    /// <summary>
    /// Defines interface for service which is working with application data storage 
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Gets or sets the path builder.
        /// </summary>
        /// <value>The path builder.</value>
        IPathBuilder PathBuilder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        /// <returns></returns>
        string GetFilePath(string fileName, FolderType folderType);

        /// <summary>
        /// Validates the name of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        bool ValidateFileName(string fileName, FolderType folderType);

        /// <summary>
        /// Determines whether [is valid file name] [the specified file name].
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        bool IsValidFileName(string fileName);

        /// <summary>
        /// Loads the text.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        string LoadText(string fileName, FolderType folderType);

        /// <summary>
        /// Saves the text.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        /// <param name="text">The text.</param>
        void SaveText(string fileName, FolderType folderType, string text);

        /// <summary>
        /// Appends the text.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        /// <param name="text">The lines.</param>
        void AppendText(string fileName, FolderType folderType, string text);

        /// <summary>
        /// Determines whether specified file exists in the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        /// <returns>
        /// 	<c>true</c> if [is file exists] [the specified file name]; otherwise, <c>false</c>.
        /// </returns>
        bool FileExists(string fileName, FolderType folderType);

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        void DeleteFile(string fileName, FolderType folderType);

        /// <summary>
        /// Deletes the files.
        /// </summary>
        /// <param name="folderType">Type of the folder.</param>
        void DeleteFiles(FolderType folderType);

        /// <summary>
        /// Finds the files.
        /// </summary>
        /// <param name="folderType">Type of the folder.</param>
        /// <param name="searchPattern">The mask.</param>
        /// <param name="fileNameOnly">if set to <c>true</c> [file name only].</param>
        /// <returns></returns>
        string[] ListFiles(FolderType folderType, string searchPattern = "*", bool fileNameOnly = true);

        /// <summary>
        /// Removes the invalid characters.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        string RemoveInvalidCharacters(string fileName);

        /// <summary>
        /// Creates the stream.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        /// <returns></returns>
        Stream CreateStream(string fileName, FolderType folderType);

        /// <summary>
        /// Deletes the folder.
        /// </summary>
        /// <param name="folderType">The diagnostics.</param>
        void DeleteFolder(FolderType folderType);

        /// <summary>
        /// Writes the CSV.
        /// </summary>
        /// <param name="csvFile">The CSV file.</param>
        /// <param name="path">The path.</param>
        void WriteCsv(CsvFile csvFile, string path);
    }
}