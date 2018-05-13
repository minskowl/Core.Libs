using System;

namespace Savchin.Services.Time
{
    public interface ITimeService
    {
        /// <summary>
        /// Gets the local.
        /// </summary>
        /// <value>
        /// The local.
        /// </value>
        TimeZoneInfo Local { get; }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TimeZoneInfo GetTimeZone(string id);

        /// <summary>
        /// Gets the now.
        /// </summary>
        /// <value></value>
        DateTime Now { get; }

        /// <summary>
        /// Gets the UTC now.
        /// </summary>
        /// <value></value>
        DateTime UtcNow { get; }
    }
}