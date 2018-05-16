using System;
using LightInject;
using Prism.Modularity;
using Savchin.Interfaces;
using Savchin.Services.Localization;
using Savchin.Services.Logging;
using Savchin.Services.Storage;
using Savchin.Services.SystemEnvironment;
using Savchin.Services.Time;

namespace Savchin.Services
{
    /// <summary>
    /// Registers common services in prism infrastructure
    /// </summary>
    public class Module : IModule
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public Module(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
        }
        
        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            _container.Register<IDispatcherTimer, CiDispatcherTimer>();
            _container.Register<ISecurityService, SecurityService>();
            _container.Register<ILocalizationLoader, XmlLocalizationLoader>();
            _container.Register<ILocalizationRegistry, LocalizationRegistry>(new PerContainerLifetime());
            _container.Register<Type, INamedLogger>((factory, value) => new NamedLogger(value.Name));
            _container.RegisterRunnableService<EnvironmentService, IEnvironmentService>();
            _container.RegisterRunnableService<TimeService, ITimeService>();
            _container.Register<IPathBuilderFactory, PathBuilderFactory>(new PerContainerLifetime());
            _container.Register<IReflectionService, ReflectionService>(new PerContainerLifetime());
        }
    }
}
