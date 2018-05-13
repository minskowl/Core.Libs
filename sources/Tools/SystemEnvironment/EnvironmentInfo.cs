using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Savchin.Collection.Generic;
using Savchin.Core;

namespace Savchin.SystemEnvironment
{
    /// <summary>
    /// EnvironmentInfo class stores information about computer  for serialize into Error Report
    /// </summary>
    public sealed class EnvironmentInfo : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the frameworks versions.
        /// </summary>
        /// <value>The frameworks versions.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public FrameworksVersionCollection FrameworksVersions { get; set; }

        /// <summary>
        /// Gets or sets the OS version.
        /// </summary>
        /// <value>The OS version.</value>
        public OperatingSystemInfo OSVersion { get; set; }

        /// <summary>
        /// Gets or sets the current culture.
        /// </summary>
        /// <value>The current culture.</value>
        public string CurrentCulture { get; set; }

        /// <summary>
        /// Gets or sets the current UI culture.
        /// </summary>
        /// <value>The current UI culture.</value>
        public string CurrentUICulture { get; }

        /// <summary>
        /// Gets or sets the name of the machine.
        /// </summary>
        /// <value>The name of the machine.</value>
        public DomainInfo DomainInfo { get; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets the UTC now.
        /// </summary>
        /// <value>The UTC now.</value>
        public DateTime UtcNow { get; private set; }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <value>
        /// The time zone.
        /// </value>
        public TimeZoneInfo TimeZone { get; }

        /// <summary>
        /// Gets the paths.
        /// </summary>
        public List<NameValuePair> Paths { get; }

        private static EnvironmentInfo _info;
        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <returns></returns>
        public static EnvironmentInfo GetCurrent()
        {
            if (_info != null)
            {
                _info.Date = DateTime.Now;
                _info.UtcNow = DateTime.UtcNow;
            }
            return _info ?? (_info = new EnvironmentInfo());
        }

        #endregion Properties

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentInfo"/> class.
        /// </summary>
        internal EnvironmentInfo()
        {
            FrameworksVersions = new FrameworksVersionCollection();
            OSVersion = new OperatingSystemInfo(Environment.OSVersion);
            CurrentCulture = Thread.CurrentThread.CurrentCulture.Name;
            CurrentUICulture = Thread.CurrentThread.CurrentUICulture.Name;
            DomainInfo = new DomainInfo();
            Date = DateTime.Now;
            UtcNow = DateTime.UtcNow;
            TimeZone = TimeZoneInfo.Local;

            Paths = new List<NameValuePair>
                {
                    new NameValuePair("CommandLine",Environment.CommandLine),
                    new NameValuePair("CurrentDirectory",Environment.CurrentDirectory)
                };
        }

        #endregion Construction

        #region Implementation of IXmlSerializable

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Date", Date.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("UtcNow", UtcNow.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("TimeZone", TimeZone.ToString());
            writer.WriteAttributeString("Culture", CurrentCulture);
            writer.WriteAttributeString("UICulture", CurrentUICulture);

            writer.WriteStartElement("OSVersion");
            ((IXmlSerializable)OSVersion).WriteXml(writer);
            writer.WriteEndElement();

            writer.WriteStartElement("Frameworks");
            foreach (var o in FrameworksVersions)
            {
                writer.WriteElementString("Framework", o.ToString());
            }

            writer.WriteEndElement();

            writer.WriteStartElement("Pathes");

            if (Paths.IsNotEmpty())
                foreach (var pair in Paths)
                {
                    writer.WriteStartElement("Path");
                    writer.WriteAttributeString("Name", pair.Name);
                    writer.WriteAttributeString("Value", pair.Value.ToString());
                    writer.WriteEndElement();
                }
            writer.WriteEndElement();

            writer.WriteStartElement("DomainInfo");
            ((IXmlSerializable)DomainInfo).WriteXml(writer);
            writer.WriteEndElement();
        }

        #endregion Implementation of IXmlSerializable
    }
}
