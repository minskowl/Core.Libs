using System;

namespace Savchin.Services
{
    public interface IReflectionService
    {
        /// <summary>
        /// Gets the assembly types from.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        Type[] GetAssemblyTypesFrom(string fileName);

        /// <summary>
        /// Gets the type from program identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Type GetTypeFromProgramId(string id);
    }
}