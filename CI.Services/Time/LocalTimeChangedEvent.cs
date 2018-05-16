using System;
using Savchin.Services.CiPrism;

namespace Savchin.Services.Time
{
    /// <summary>
    /// This event is raised by application in order to notify interested parties
    /// that local time was changed
    /// </summary>
    public class LocalTimeChangedEvent : CiPubSubEvent<DateTime>
    {
    }
}
