using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Savchin.Collection.Generic;
using Savchin.Core;
using Savchin.Logging;

namespace Savchin.SystemEnvironment
{
    public sealed class HardwareInfo : IXmlSerializable
    {
        #region Fields

        private readonly ILogger _logger;
        private static HardwareInfo _info;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the processor.
        /// </summary>
        /// <value>
        /// The processor.
        /// </value>
        public List<NameValuePair> Processor { get; }

        /// <summary>
        /// Gets the memory.
        /// </summary>
        /// <value>
        /// The memory.
        /// </value>
        public List<NameValuePair> Memory { get; }

        /// <summary>
        /// Gets the mother board.
        /// </summary>
        /// <value>
        /// The mother board.
        /// </value>
        public List<NameValuePair> Motherboard { get; }

        /// <summary>
        /// Gets the screens.
        /// </summary>
        /// <value>
        /// The screens.
        /// </value>
        public List<NameValuePair> Screens { get; }

        #endregion Properties

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="HardwareInfo"/> class.
        /// </summary>
        internal HardwareInfo(ILogger logger)
        {
            _logger = logger;

            Processor = GetData("Win32_Processor", "Name", "ProcessorId");
            Memory = GetData("Win32_PhysicalMemory", "Capacity");
            Motherboard = GetData("Win32_BaseBoard", "Manufacturer", "Product");

            //Screens = new List<NameValuePair>
            //{
            //    new NameValuePair("DpiX", SystemParametersHelper.GetDpiX()),
            //    new NameValuePair("DpiY", SystemParametersHelper.GetDpiY())
            //};

            //foreach (var screen in Screen.AllScreens)
            //{
            //    Screens.Add(new NameValuePair("DeviceName", screen.DeviceName));
            //    Screens.Add(new NameValuePair("Bounds", screen.Bounds));
            //    Screens.Add(new NameValuePair("WorkingArea", screen.WorkingArea));
            //    Screens.Add(new NameValuePair("PrimaryScreen", screen.Primary));
            //}
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HardwareInfo"/> class.
        /// </summary>
        internal HardwareInfo()
        {
            Processor = new List<NameValuePair>();
            Memory = new List<NameValuePair>();
            Motherboard = new List<NameValuePair>();
            Screens = new List<NameValuePair>();
        }

        #endregion Construction

        #region Public methods

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <returns></returns>
        public static HardwareInfo GetCurrent(ILogger logger)
        {
            return _info ?? (_info = new HardwareInfo(logger));
        }

        #endregion Public methods

        #region Helper methods

        private List<NameValuePair> GetData(string table, params string[] properties)
        {
            var query = $"SELECT {string.Join(",", properties)} FROM {table}";

            var result = new List<NameValuePair>();
            try
            {
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (var obj in searcher.Get())
                    {
                        Array.ForEach(properties, e => result.Add(new NameValuePair(e, obj[e].ToString().Trim())));
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Warning("Failed to obtain hardware info: " + exception);
            }

            return result;
        }

        #endregion Helper methods

        #region Implementation of IXmlSerializable

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            WriteCollection(writer, Processor, "Processor");
            WriteCollection(writer, Memory, "Memory");
            WriteCollection(writer, Motherboard, "MotherBoard");
            WriteCollection(writer, Screens, "Screens");
        }

        private static void WriteCollection(XmlWriter writer, List<NameValuePair> collection, string name)
        {
            writer.WriteStartElement(name);

            if (collection.IsNotEmpty())
                foreach (var pair in collection)
                {
                    writer.WriteElementString(pair.Name, pair.Value.ToString());

                }
            writer.WriteEndElement();
        }

        #endregion Implementation of IXmlSerializable
    }
}
