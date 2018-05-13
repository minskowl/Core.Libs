namespace Savchin.Services.Storage
{
    /// <summary>
    /// Builds path which is not user-aware
    /// </summary>
    public class CommonPathBuilder : PathBuilder
    {
        #region Constants

        private const string CommonFolderPath = "Common";

        #endregion

        #region Overriden methods

        /// <summary>
        /// Gets the sub folder path.
        /// </summary>
        /// <param name="folderType">Type of the folder.</param>
        protected override string GetSubfolderPath(FolderType folderType)
        {
            return CommonFolderPath + "\\" + base.GetSubfolderPath(folderType);
        }

        #endregion
    }
}