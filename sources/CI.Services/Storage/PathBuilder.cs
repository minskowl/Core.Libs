using System.IO;
using System.Reflection;

namespace Savchin.Services.Storage
{
    /// <summary>
    /// Implements basic path builder logic
    /// </summary>
    public class PathBuilder : IPathBuilder
    {
        #region Constants

        private readonly string _dataFolderPath;

        #endregion Constants

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PathBuilder" /> class.
        /// </summary>
        public PathBuilder() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathBuilder" /> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public PathBuilder(Assembly assembly)
        {
            _dataFolderPath = PathProvider.GetDataFolder(assembly ?? PathProvider.DefaultAssembly);
        }

        #endregion Construction

        #region Implementation of IPathBuilder

        /// <summary>
        /// Gets the folder path.
        /// </summary>
        /// <param name="folderType">Type of the folder.</param>
        public string GetFolderPath(FolderType folderType)
        {
            if (folderType == FolderType.Root)
                return _dataFolderPath;

            var subFolder = GetSubfolderPath(folderType);
            return string.IsNullOrEmpty(subFolder) ? null : Path.Combine(_dataFolderPath, subFolder);
        }

        #endregion Implementation of IPathBuilder

        #region Virtual methods

        protected virtual string GetSubfolderPath(FolderType folderType)
        {
            return folderType.ToString();
        }

        #endregion Virtual methods
    }
}