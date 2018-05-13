using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using Savchin.Core;

namespace Savchin.Services.Storage
{
    public static class PathProvider
    {
        /// <summary>
        /// Gets or sets the broker.
        /// </summary>
        /// <value>
        /// The broker.
        /// </value>
        public static string Broker { get; set; }

        /// <summary>
        /// Gets the default assembly.
        /// </summary>
        /// <value>
        /// The default assembly.
        /// </value>
        public static Assembly DefaultAssembly
        {
            get { return Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly(); }
        }

        /// <summary>
        /// Gets the scheduled package folder path.
        /// </summary>
        public static string ScheduledPackageFolderPath
        {
            get { return Path.Combine(GetDataFolder(DefaultAssembly), FolderType.ScheduledPackage.ToString()); }
        }

        /// <summary>
        /// Gets the logs folder path.
        /// </summary>
        /// <returns></returns>
        public static string LogsFolderPath 
        {
            get { return Path.Combine(GetDataFolder(DefaultAssembly), FolderType.Logs.ToString()); }
        }

        /// <summary>
        /// Gets the broker path.
        /// </summary>
        /// <returns></returns>
        public static string BrokerPath
        {
            get { return SettingsPath(DefaultAssembly); }
        }

        /// <summary>
        /// Gets the broker relative path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetBrokerRelativePath(string path)
        {
            var brokerPath = BrokerPath + "\\";
            return path.Contains(brokerPath) ? path.Replace(brokerPath, string.Empty) : null;
        }

        /// <summary>
        /// Gets the data folder.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string GetDataFolder(Assembly assembly)
        {
            var version = assembly.GetName().Version;
            var title = assembly.GetAttribute<AssemblyTitleAttribute>()?.Title ?? string.Empty;
            var settingsFolder = assembly.GetAttribute<AssemblySettingsFolderAttribute>()?.SettingsFolder ?? string.Empty;

            return Path.Combine(
                SettingsPath(assembly),
                string.IsNullOrEmpty(settingsFolder) ? title : settingsFolder,
                "v" + version.Major + "." + version.Minor);
        }

        private static string SettingsPath(Assembly assembly)
        {
            if (ConfigurationManager.AppSettings.Get("Portable") == "True")
            {
                return Path.GetDirectoryName(assembly.Location);
            }

            var curAssembly = assembly ?? DefaultAssembly;
            var companyAttr = curAssembly.GetAttribute<AssemblyCompanyAttribute>();

            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                companyAttr == null ? string.Empty : companyAttr.Company,
                Broker ?? string.Empty);
        }
    }
}
