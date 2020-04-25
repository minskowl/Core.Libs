using System;
using Savchin.Interfaces;

namespace Savchin.ComponentModel
{
    public static class ServiceLocator
    {
        private static IContainer _container;

        /// <summary>
        /// Gets the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public static T Get<T>(ref T value) where T : class
        {
            return value ?? (value = GetInstance<T>());
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object GetInstance(Type serviceType)
        {
            return  _container.GetInstance(serviceType);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>() where T : class
        {
            return _container.GetInstance(typeof(T)) as T;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T GetInstance<T>(string key) where T : class
        {
            return _container.GetInstance(typeof(T), key) as T;
        }
    }
}
