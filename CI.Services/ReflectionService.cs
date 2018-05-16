using System;
using System.Reflection;

namespace Savchin.Services
{
    public class ReflectionService : IReflectionService
    {
        /// <summary>
        /// Gets the assembly types from.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods")]
        public Type[] GetAssemblyTypesFrom(string fileName)
        {
            var assembly = Assembly.LoadFrom(fileName);
            return assembly.GetTypes();
        }

        /// <summary>
        /// Gets the type from program identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Type GetTypeFromProgramId(string id)
        {
            return Type.GetTypeFromProgID(id);
        }
    }
}