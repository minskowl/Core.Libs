using System;
using Savchin.Logging;

namespace Savchin.Services.Execution
{
    public abstract class ServerRequestBase<TResponse, TClient> : RequestBase<TResponse>
        where TClient : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerRequestBase{TResponse, TClient}"/> class.
        /// </summary>
        /// <param name="loggerFacade">The logger facade.</param>
        protected ServerRequestBase(ILogger loggerFacade) : base(loggerFacade)
        {
        }

        protected override TResponse GetResult()
        {
            using (var client = CreateClient())
                return InvokeService(client);
        }

        /// <summary>
        /// Invokes the service.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        protected abstract TResponse InvokeService(TClient client);

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <returns></returns>
        protected abstract TClient CreateClient();
    }
}