using System.Globalization;
using Savchin.Services.CiPrism;

namespace Savchin.Services.SystemEnvironment
{
    public class ChangeLocaleEvent : CiPubSubEvent<CultureInfo>
    {
    }
}
