using System;
using System.Runtime.InteropServices;

namespace Savchin.Services.Storage
{
    [AttributeUsage(AttributeTargets.Assembly)]
    [ComVisible(true)]
    public sealed class AssemblySettingsFolderAttribute : Attribute
    {
        public string SettingsFolder { get; private set; }

        public AssemblySettingsFolderAttribute(string settingsFolder)
        {
            SettingsFolder = settingsFolder;
        }
        public AssemblySettingsFolderAttribute()
        {
            // Code Analysis CA1409 warning fix
        }
    }
}