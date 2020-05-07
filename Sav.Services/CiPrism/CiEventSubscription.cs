using System;
using Prism.Events;
using Savchin.Services.Logging;

namespace Savchin.Services.CiPrism
{
   public class CiEventSubscription<TPayload> : EventSubscription<TPayload>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CiEventSubscription{TPayload}"/> class.
        /// </summary>
        /// <param name="actionReference">The action reference.</param>
        /// <param name="filterReference">The filter reference.</param>
        public CiEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
            : base(actionReference, filterReference)
        {
        }
        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="argument">The argument.</param>
        public override void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            try
            {
                action(argument);
            }
            catch (Exception ex)
            {
                AppLogger.Default.Error(ex);
            }
        }
    }
}