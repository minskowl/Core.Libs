using LightInject;

namespace Savchin.ComponentModel
{
    /// <summary>
    /// Container Helper class
    /// </summary>
    public static class ContainerHelper
    {
        /// <summary>
        /// Registers the multiple.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TInterface1">The type of the interface1.</typeparam>
        /// <typeparam name="TInterface2">The type of the interface2.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void RegisterMultiple<TService, TInterface1, TInterface2>(this IServiceRegistry container, ILifetime lifetime = null, string key1 = null, string key2 = null)
            where TService : TInterface1, TInterface2
        {
            container.Register<TService>(lifetime);
            container.Register(f => (TInterface1)f.GetInstance<TService>(), key1 ?? string.Empty);
            container.Register(f => (TInterface2)f.GetInstance<TService>(), key2 ?? string.Empty);
        }

        /// <summary>
        /// Registers the multiple.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TInterface1">The type of the interface1.</typeparam>
        /// <typeparam name="TInterface2">The type of the interface2.</typeparam>
        /// <typeparam name="TInterface3">The type of the interface3.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <param name="key3">The key3.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void RegisterMultiple<TService, TInterface1, TInterface2, TInterface3>(this IServiceRegistry container, ILifetime lifetime = null, string key1 = null, string key2 = null, string key3 = null)
            where TService : TInterface1, TInterface2, TInterface3
        {
            container.Register<TService>(lifetime);
            container.Register(f => (TInterface1)f.GetInstance<TService>(), key1 ?? string.Empty);
            container.Register(f => (TInterface2)f.GetInstance<TService>(), key2 ?? string.Empty);
            container.Register(f => (TInterface3)f.GetInstance<TService>(), key3 ?? string.Empty);
        }

        /// <summary>
        /// Registers the multiple.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TInterface1">The type of the interface1.</typeparam>
        /// <typeparam name="TInterface2">The type of the interface2.</typeparam>
        /// <typeparam name="TInterface3">The type of the interface3.</typeparam>
        /// <typeparam name="TInterface4">The type of the interface4.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <param name="key3">The key3.</param>
        /// <param name="key4">The key4.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void RegisterMultiple<TService, TInterface1, TInterface2, TInterface3, TInterface4>(this IServiceRegistry container, ILifetime lifetime = null, string key1 = null, string key2 = null, string key3 = null, string key4 = null)
            where TService : TInterface1, TInterface2, TInterface3, TInterface4
        {
            container.Register<TService>(lifetime);
            container.Register(f => (TInterface1)f.GetInstance<TService>(), key1 ?? string.Empty);
            container.Register(f => (TInterface2)f.GetInstance<TService>(), key2 ?? string.Empty);
            container.Register(f => (TInterface3)f.GetInstance<TService>(), key3 ?? string.Empty);
            container.Register(f => (TInterface4)f.GetInstance<TService>(), key4 ?? string.Empty);
        }
    }
}
