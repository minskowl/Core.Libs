using System;
using System.IO;
using System.Linq;
using Savchin.Csv;
using Savchin.Interfaces;

namespace Savchin.Services.Storage
{
    /// <summary>
    /// Implements service which is working with application data storage 
    /// </summary>
    public class StorageService : IStorageService
    {
        #region Properties

        private readonly string[] _invalidCharacters = {".", "_"};

        private readonly IFileSystem _fileSystem;
        
        private IPathBuilder _pathBuilder;

        /// <summary>
        /// Gets or sets the path builder.
        /// </summary>
        /// <value>The path builder.</value>
        public IPathBuilder PathBuilder
        {
            get { return _pathBuilder; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _pathBuilder = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageService" /> class.
        /// </summary>
        /// <param name="pathBuilder">The path builder.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <exception cref="System.ArgumentNullException">pathBuilder</exception>
        public StorageService(IPathBuilder pathBuilder, IFileSystem fileSystem)
        {
            if (pathBuilder == null)
                throw new ArgumentNullException("pathBuilder");

            PathBuilder = pathBuilder;
            _fileSystem = fileSystem;
        }

        #endregion

        #region Implementation of IStorageSevice

        /// <summary>
        /// Validates the name of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        /// <returns></returns>
        public bool ValidateFileName(string fileName, FolderType folderType)
        {
            if (!IsValidFileName(fileName))
                throw new ArgumentException("File name has invalid characters \\ / : ? <> | * _ .", "fileName");
            return GetFile(fileName, folderType) != null;
        }

        /// <summary>
        /// Determines whether [is valid file name] [the specified file name].
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public bool IsValidFileName(string fileName)
        {
            return _fileSystem.File.IsValidFileName(fileName) && !_invalidCharacters.Any(fileName.Contains);
        }


        /// <summary>
        /// Loads the text.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        public string LoadText(string fileName, FolderType folderType)
        {
            var file = GetFile(fileName, folderType);

            return file == null || !file.Exists ? string.Empty : File.ReadAllText(file.FullName);
        }

        /// <summary>
        /// Saves the text.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        /// <param name="text">The text.</param>
        public void SaveText(string fileName, FolderType folderType, string text)
        {
            var file = GetFile(fileName, folderType);

            if (file == null || file.DirectoryName == null) return;

            if (!Directory.Exists(file.DirectoryName))
                Directory.CreateDirectory(file.DirectoryName);

            try
            {
                File.WriteAllText(file.FullName, text);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Can't access to file " + file.FullName, ex);
            }

        }


        /// <summary>
        /// Appends the text.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        /// <param name="text">The text.</param>
        public void AppendText(string fileName, FolderType folderType, string text)
        {
           File.AppendAllText(GetFilePath(fileName,FolderType.Diagnostics),text);
        }


        /// <summary>
        /// Determines whether specified file exists in the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        /// <returns>
        /// 	<c>true</c> if [is file exists] [the specified file name]; otherwise, <c>false</c>.
        /// </returns>
        public bool FileExists(string fileName, FolderType folderType)
        {
            return File.Exists(GetFilePath(fileName, folderType));
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderType">Type of the folder.</param>
        public void DeleteFile(string fileName, FolderType folderType)
        {
            var path = GetFilePath(fileName, folderType);
            _fileSystem.File.Delete(path);
        }


        public string GetFilePath(string fileName, FolderType folderType)
        {
            var folderPath = PathBuilder.GetFolderPath(folderType);
            if (!string.IsNullOrEmpty(folderPath) && !_fileSystem.Directory.Exists(folderPath))
                _fileSystem.Directory.CreateDirectory(folderPath);

            return string.IsNullOrEmpty(folderPath) ? null : Path.Combine(folderPath, fileName);
        }

        /// <summary>
        /// Finds the files.
        /// </summary>
        /// <param name="folderType">Type of the folder.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="fileNameOnly">if set to <c>true</c> [file name only].</param>
        /// <returns></returns>
        public string[] ListFiles(FolderType folderType, string searchPattern="*", bool fileNameOnly = true)
        {
            var path = PathBuilder.GetFolderPath(folderType);

            if (!Directory.Exists(path))
                return new string[0];
            var files = Directory.GetFiles(path, searchPattern);
            return fileNameOnly ? files.Select(Path.GetFileName).ToArray() : files;
        }

        /// <summary>
        /// Removes the invalid characters.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string RemoveInvalidCharacters(string fileName)
        {
            return _invalidCharacters.Aggregate(fileName, (current, character) => current.Replace(character, string.Empty));
        }

        public Stream CreateStream(string fileName, FolderType folderType)
        {
            return _fileSystem.File.OpenStream(GetFilePath(fileName, folderType));
        }

        /// <summary>
        /// Deletes the folder.
        /// </summary>
        /// <param name="folderType">Type of the folder.</param>
        public void DeleteFolder(FolderType folderType)
        {
            var path = PathBuilder.GetFolderPath(folderType);
            if (Directory.Exists(path))
                Directory.Delete(PathBuilder.GetFolderPath(folderType),true);
        }

        /// <summary>
        /// Writes the CSV.
        /// </summary>
        /// <param name="csvFile">The CSV file.</param>
        /// <param name="path">The path.</param>
        public void WriteCsv(CsvFile csvFile, string path)
        {
            using (var writer = new CsvWriter())
            {
                writer.WriteCsv(csvFile, path);
            }
        }

        /// <summary>
        /// Deletes the files.
        /// </summary>
        /// <param name="folderType">Type of the folder.</param>
        public void DeleteFiles(FolderType folderType)
        {
            var path = PathBuilder.GetFolderPath(folderType);

            _fileSystem.Directory.ClearFiles(path);
        }

        #endregion

        #region Helper methods

    
        private FileInfo GetFile(string fileName, FolderType folderType)
        {
            var path = GetFilePath(fileName, folderType);
            return string.IsNullOrEmpty(path) ? null : new FileInfo(path);
        }

        #endregion
    }
}
