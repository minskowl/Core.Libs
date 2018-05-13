using System.Reflection;

namespace Savchin.Services.Storage
{
    public interface IPathBuilderFactory
    {
        /// <summary>
        /// Creates the path builder.
        /// </summary>
        /// <returns></returns>
        IPathBuilder CreatePathBuilder();

        /// <summary>
        /// Creates the path builder.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        IPathBuilder CreatePathBuilder(Assembly assembly);

        /// <summary>
        /// Creates the common path builder.
        /// </summary>
        /// <returns></returns>
        IPathBuilder CreateCommonPathBuilder();
    }
}
