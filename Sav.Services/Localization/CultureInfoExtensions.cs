using System.Globalization;
using Savchin.Text;

namespace Savchin.Services.Localization
{
    public static class CultureInfoExtensions
    {
        public static string GetShortNativeName(this CultureInfo cultureInfo)
        {
            var name = string.IsNullOrEmpty(cultureInfo.Parent.Name) ? cultureInfo.NativeName : cultureInfo.Parent.NativeName;
            return StringUtil.CapitalizeFirstLatter(name);
        }
    }
}
