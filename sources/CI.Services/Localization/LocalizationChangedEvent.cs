using System;
using Savchin.Services.CiPrism;

namespace Savchin.Services.Localization
{
    /// <summary>
    /// This event is raised when current culture is changed
    /// </summary>
    public class LocalizationChangedEvent : CiPubSubEvent<EventArgs>
    {
    }
}
