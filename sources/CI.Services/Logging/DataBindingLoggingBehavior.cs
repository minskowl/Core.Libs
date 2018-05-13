using System.Diagnostics;
using Savchin.Logging;

namespace Savchin.Services.Logging
{
    public class DataBindingLoggingBehavior : TraceListener, IBehavior
    {
        private readonly ILogger _logger;

        public DataBindingLoggingBehavior(ILogger logger)
        {
            _logger = logger;
       //     PresentationTraceSources.Refresh();
   //         PresentationTraceSources.DataBindingSource.Listeners.Add(this);
            //PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
         //   PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error | SourceLevels.Warning;
        }

        public override void Write(string message)
        {
            _logger.Warning(message);
        }

        public override void WriteLine(string message)
        {
            _logger.Warning(message);
        }
    }


    
}
