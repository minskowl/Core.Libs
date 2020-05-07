using System;


namespace Savchin.Services
{
    public interface IDispatcher
    {

        ///// <summary>
        ///// Begins the invoke.
        ///// </summary>
        ///// <param name="method">The method.</param>
        ///// <param name="priority">The priority.</param>
        ///// <param name="arg">The argument.</param>
        //DispatcherOperation BeginInvoke(Delegate method, DispatcherPriority priority, params object[] arg);
        void BeginInvoke(Delegate method);
        /// <summary>
        /// Begins the invoke.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="arg">The arg.</param>
        void BeginInvoke(Delegate method, object arg);

        /// <summary>
        /// Invokes the specified callback.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        TResult Invoke<TResult>(Func<TResult> callback);

        ///// <summary>
        ///// Begins the invoke.
        ///// </summary>
        ///// <param name="method">The method.</param>
        //DispatcherOperation BeginInvoke(Action method);

        ///// <summary>
        ///// Occurs when [operation posted].
        ///// </summary>
        //event DispatcherHookEventHandler OperationPosted;

        ///// <summary>
        ///// Occurs when [operation aborted].
        ///// </summary>
        //event DispatcherHookEventHandler OperationAborted;



        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <returns></returns>
        bool CheckAccess();

        /// <summary>
        /// Does the events.
        /// </summary>
        void DoEvents();
    }

    public static class DispatcherHelper
    {
        public static void EasyBeginInvoke(this IDispatcher dispatcher,Delegate method, object arg)
        {
            if (dispatcher.CheckAccess())
            {
                 method.Method.Invoke(method.Target,new [] { arg});
            }
            else
            {
                dispatcher.BeginInvoke(method, arg);
            }
        }
    }
}
