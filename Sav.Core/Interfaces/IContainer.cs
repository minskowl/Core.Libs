using System;
using LightInject;

namespace Savchin.Interfaces
{
    /// <summary>
    /// Abstraction for container
    /// </summary>
    public interface IContainer : IServiceRegistry, IServiceFactory, IDisposable
    {

    }

    public interface IServiceRegistry : LightInject.IServiceRegistry
    {
        void RegisterInstance<TService>(TService instance, ILifetime lifetime);

        void RegisterInstance(Type serviceType, object value, ILifetime lifetime);

        void RegisterNamed<TService, TImplementation>() where TImplementation : TService;
    }
}