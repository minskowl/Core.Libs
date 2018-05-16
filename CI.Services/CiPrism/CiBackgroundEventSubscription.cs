using System;
using System.Threading.Tasks;
using Prism.Events;
using Savchin.Services.Logging;

namespace Savchin.Services.CiPrism
{
    public class CiBackgroundEventSubscription<TPayload> : BackgroundEventSubscription<TPayload>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CiBackgroundEventSubscription{TPayload}"/> class.
        /// </summary>
        /// <param name="actionReference">The action reference.</param>
        /// <param name="filterReference">The filter reference.</param>
        public CiBackgroundEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference) : base(actionReference, filterReference)
        {
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="argument">The argument.</param>
        public override void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            Task.Run(() =>
            {
                try
                {
                    action(argument);
                }
                catch (Exception ex)
                {
                    AppLogger.Default.Error(ex);
                }
            });
        }
    }
}