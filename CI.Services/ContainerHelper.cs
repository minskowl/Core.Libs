using LightInject;
using Savchin.ComponentModel;
using Savchin.Services.Execution;
using IServiceRegistry = LightInject.IServiceRegistry;

namespace Savchin.Services
{
    public static class ContainerHelper
    {
        /// <summary>
        /// Registers the behavior.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container">The container.</param>
        public static void RegisterBehavior<T>(this IServiceRegistry container)
            where T : IBehavior
        {
            container.Register<IBehavior, T>(typeof(T).FullName, new PerContainerLifetime());
        }
        
        /// <summary>
        /// Registers the runnable service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="container">The container.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void RegisterRunnableService<TService, TInterface>(this IServiceRegistry container)
            where TService : IRunnable, TInterface
        {
            container.RegisterMultiple<TService, TInterface, IRunnable>(new PerContainerLifetime());
        }

        /// <summary>
        /// Registers the runnable.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <param name="container">The container.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void RegisterRunnable<TInstance>(this IServiceRegistry container)
            where TInstance : IRunnable
        {
            container.Register<IRunnable, TInstance>(typeof(TInstance).FullName, new PerContainerLifetime());
        }
    }
}
