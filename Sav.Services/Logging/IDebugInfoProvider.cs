namespace Savchin.Services.Logging
{
    internal interface IDebugInfoProvider
    {
        /// <summary>
        /// Gets the debug information.
        /// </summary>
        /// <returns></returns>
        object GetDebugInfo();
    }
}