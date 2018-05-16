using System;
using System.Collections.Generic;
using log4net.Core;
using log4net.Filter;

namespace Savchin.Services.Logging
{
    public class AndFilter : FilterSkeleton
    {
        private readonly IList<IFilter> _filters = new List<IFilter>();

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
                throw new ArgumentNullException("loggingEvent");

            foreach (var filter in _filters)
            {
                if (filter.Decide(loggingEvent) != FilterDecision.Accept)
                    return FilterDecision.Neutral; // one of the filter has failed
            }

            return  FilterDecision.Accept;
        }

        public void AddFilter(IFilter filter)
        {
            _filters.Add(filter);
        }
    }
}