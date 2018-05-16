using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Savchin.Services.Localization
{
    public interface ILocalizationLoader
    {
        IEnumerable<ILocalizationProvider> SelectProviders();
    }

    public abstract class LocalizationLoaderBase : ILocalizationLoader
    {
        private readonly Dictionary<string, PropertyInfo> _propertyMap;
        private readonly Type _localizationObjectType;


        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationLoaderBase"/> class.
        /// </summary>
        /// <param name="localizationObjectType">Type of the localization object.</param>
        protected LocalizationLoaderBase(Type localizationObjectType)
        {
            _localizationObjectType = localizationObjectType;
            _propertyMap = _localizationObjectType.GetProperties().ToDictionary(e => e.Name, e => e);
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ILocalizationProvider> SelectProviders()
        {
            return LoadTranslations().Select(CreateProvider);
        }

        protected abstract IEnumerable<Translations> LoadTranslations();


        private ILocalizationProvider CreateProvider(Translations translations)
        {
            var obj = Activator.CreateInstance(_localizationObjectType);

            foreach (var pair in translations.Map)
            {
                var property = _propertyMap[pair.Key];
                property.SetValue(obj, pair.Value, null);
            }
            return new LocalizationProvider(obj, translations);
        }
    }
}