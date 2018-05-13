using System.Threading;
using Savchin.Logging;

namespace Savchin.Services.Execution
{
    public class WaitTask : RequestBase<object>
    {
        public WaitHandle[] Handlers { get; set; }

        public WaitTask(ILogger loggerFacade) : base(loggerFacade)
        {
        }

        protected override object GetResult()
        {
            WaitHandle.WaitAll(Handlers);
            return null;
        }
    }
}
