using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Events;
using Savchin.Services.Logging;

namespace Savchin.Services.CiPrism
{

    public class CiPubSubEvent<TPayload> : PubSubEvent<TPayload>
    {
        private static readonly Predicate<TPayload> EmptyPredicate = delegate { return true; };
        private static readonly Dictionary<object, SubscriptionToken> Tokens = new Dictionary<object, SubscriptionToken>();

        /// <summary>
        /// Subscribes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="threadOption">The thread option.</param>
        /// <param name="keepSubscriberReferenceAlive">if set to <c>true</c> [keep subscriber reference alive].</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public override SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TPayload> filter)
        {
            IDelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);
            IDelegateReference filterReference = filter != null
                ? new DelegateReference(filter, keepSubscriberReferenceAlive)
                : new DelegateReference(EmptyPredicate, true);

            EventSubscription<TPayload> subscription;
            switch (threadOption)
            {
                case ThreadOption.PublisherThread:
                    subscription = new CiEventSubscription<TPayload>(actionReference, filterReference);
                    break;
                case ThreadOption.BackgroundThread:
                    subscription = new CiBackgroundEventSubscription<TPayload>(actionReference, filterReference);
                    break;
                case ThreadOption.UIThread:
                    subscription = new CiDispatcherEventSubscription<TPayload>(actionReference, filterReference, SynchronizationContext);
                    break;
                default:
                    subscription = new CiEventSubscription<TPayload>(actionReference, filterReference);
                    break;
            }

            return InternalSubscribe(subscription);
        }

        public virtual SubscriptionToken SubscribeTask(Func<TPayload, Task> task, ThreadOption threadOption = ThreadOption.PublisherThread)
        {
            var action = new Action<TPayload>(payload =>
            {
                task(payload).ContinueWith(x =>
                {
                    AppLogger.Default.Error(x.Exception);
                }, TaskContinuationOptions.OnlyOnFaulted);
            });
            var unsToken = Subscribe(action, threadOption, true, null);
            lock (Tokens)
            {
                Tokens.Add(task, unsToken);
            }
            return unsToken;
        }

        /// <summary>
        /// Subscribes the specified action.
        /// </summary>
        /// <returns></returns>
        public virtual void UnsubscribeTask(Func<TPayload, Task> task)
        {
            lock (Tokens)
            {
                SubscriptionToken token;
                if (Tokens.TryGetValue(task, out token))
                {
                    Unsubscribe(token);
                    Tokens.Remove(task);
                }
            }
        }
    }
}