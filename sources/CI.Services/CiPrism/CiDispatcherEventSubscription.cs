using System;
using System.Threading;
using Prism.Events;
using Savchin.Services.Logging;

namespace Savchin.Services.CiPrism
{
    public class CiDispatcherEventSubscription<TPayload> : DispatcherEventSubscription<TPayload>
    {
        private readonly SynchronizationContext _syncContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CiDispatcherEventSubscription{TPayload}"/> class.
        /// </summary>
        /// <param name="actionReference">The action reference.</param>
        /// <param name="filterReference">The filter reference.</param>
        /// <param name="context">The context.</param>
        public CiDispatcherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference, SynchronizationContext context) 
            : base(actionReference, filterReference, context)
        {
            _syncContext = context;
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="argument">The argument.</param>
        public override void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            _syncContext.Post(o =>
            {
                try
                {
                    action((TPayload) o);
                }
                catch (Exception ex)
                {
                    AppLogger.Default.Error(ex);
                }
            }, argument);
        }

    }
}