using System.Collections.Generic;
using System.IO;

namespace Savchin.Services.Storage
{
    /// <summary>
    /// StorageUtility
    /// </summary>
    public static class StorageUtility
    {
        private static Dictionary<string,string> _brandFixes=new Dictionary<string, string>();

        public static void RegisterFix(string oldName, string newName)
        {
            _brandFixes[newName] = oldName;
        }

        private static void FixBrandName(string oldName, string newName)
        {
            var tmpBroker = PathProvider.Broker;
            var oldBrandName = GetBrokerPath(oldName);
            var newBrandName = GetBrokerPath(newName);
            PathProvider.Broker = tmpBroker;

            if (Directory.Exists(oldBrandName) && !Directory.Exists(newBrandName))
                Directory.Move(oldBrandName, newBrandName);
        }

        public static void FixBrand(string name)
        {
            string oldName;
            if (_brandFixes.TryGetValue(name, out oldName))
                FixBrandName(oldName,name);
        }

        private static string GetBrokerPath(string broker)
        {
            PathProvider.Broker = broker;
            return PathProvider.BrokerPath;
        }
    }
}
