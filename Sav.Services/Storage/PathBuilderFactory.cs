using System.Reflection;

namespace Savchin.Services.Storage
{
    public class PathBuilderFactory : IPathBuilderFactory
    {
        /// <summary>
        /// Creates the path builder.
        /// </summary>
        /// <returns></returns>
        public IPathBuilder CreatePathBuilder()
        {
            return new PathBuilder();
        }

        /// <summary>
        /// Creates the path builder.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public IPathBuilder CreatePathBuilder(Assembly assembly)
        {
            return new PathBuilder(assembly);
        }

        /// <summary>
        /// Creates the common path builder.
        /// </summary>
        /// <returns></returns>
        public IPathBuilder CreateCommonPathBuilder()
        {
            return new CommonPathBuilder();
        }
    }
}