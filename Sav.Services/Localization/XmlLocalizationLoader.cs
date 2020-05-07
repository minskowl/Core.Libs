using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Savchin.Text;

namespace Savchin.Services.Localization
{
    public class XmlLocalizationLoader : LocalizationLoaderBase
    {

        private readonly Assembly _assembly;
        private readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlLocalizationLoader"/> class.
        /// </summary>
        public XmlLocalizationLoader()
            : this(typeof(XmlLocalizationLoader).Assembly, ".Localization.CILocalization.xml", typeof(CI.Services.Localization.AutoGenerate.Localization))
        {

        }

        public XmlLocalizationLoader(Assembly assembly, string path, Type localizationObjectType)
            : base(localizationObjectType)
        {
            _path = path;
            _assembly = assembly;
        }

        public XElement Load()
        {
            var path = _assembly.GetName().Name + _path;
            using (var stream = _assembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                    throw new InvalidOperationException($"Try to load localization resource '{path}' but exists{_assembly.GetManifestResourceNames().Join(Environment.NewLine)}");
                return XElement.Load(stream);
            }
        }


        protected override IEnumerable<Translations> LoadTranslations()
        {
            var root = Load();

            var languages = root.Descendants("s").SelectMany(e => e.Descendants()).Select(e => e.Name.LocalName).Distinct().ToArray();
            var res = languages.ToDictionary(e => e, e => new Translations(new CultureInfo(e)));


            foreach (var s in root.Descendants("s"))
            {
                var id = s.Attribute("id").Value;
                string defaultValue = null;

                foreach (var loc in s.Elements())
                {
                    var lang = loc.Name.LocalName;
                    var text = loc.Value;
                    if (lang == "en-GB")
                        defaultValue = text;
                    res[lang].Map[id] = text;
                }
                if (!string.IsNullOrWhiteSpace(defaultValue))
                {
                    foreach (var lang in languages.Except(s.Elements().Select(e => e.Name.LocalName)))
                    {
                        res[lang].Map[id] = defaultValue;
                    }
                }
            }

            foreach (var re in res)
            {
                var translations = re.Value;
                translations.Id = int.Parse(translations.Map["Id"]);
                yield return translations;
            }

        }


    }
}