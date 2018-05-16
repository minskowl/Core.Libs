using System;
using System.Collections.Generic;
using LightInject;
using Microsoft.Practices.ServiceLocation;

namespace Savchin.Services.Core
{
    public class LightInjectServiceLocator : ServiceLocatorImplBase
    {
        private readonly IServiceContainer _serviceContainer;
        public LightInjectServiceLocator(IServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
        }
        protected override object DoGetInstance(Type serviceType, string key)
        {
            return key != null 
                ? _serviceContainer.GetInstance(serviceType, key) 
                : _serviceContainer.GetInstance(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return _serviceContainer.GetAllInstances(serviceType);
        }
    }
}
