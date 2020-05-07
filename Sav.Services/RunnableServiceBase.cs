using System;
using Savchin.Logging;

namespace Savchin.Services.Core
{
    public class RunnableServiceBase : ServiceBase, IDisposable
    {
        public RunnableServiceBase(ILogger logger)
            : base(logger)
        {
        }

        public virtual void Start()
        {

        }

        public virtual void Stop()
        {

        }

        protected virtual void Dispose(bool disposing)
        {

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RunnableServiceBase()
        {
            Dispose(false);
        }
    }
}
