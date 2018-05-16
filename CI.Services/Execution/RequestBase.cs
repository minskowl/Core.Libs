using System;
using Savchin.Logging;

namespace Savchin.Services.Execution
{
    /// <summary>
    /// Defines strictly typed functionality for request
    /// </summary>
    /// <typeparam name="T">Type of result</typeparam>
    public abstract class RequestBase<T> : RequestBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the on complete.
        /// </summary>
        /// <value>The on complete.</value>
        public Action<T> OnResult { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBase&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="loggerFacade">The logger facade.</param>
        protected RequestBase(ILogger loggerFacade)
            : base(loggerFacade)
        {

        }

        #endregion

        /// <summary>
        /// Executes this instance.
        /// </summary>
        protected override void Execute()
        {
            try
            {
                HandleResult(GetResult());
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <returns></returns>
        protected abstract T GetResult();


        /// <summary>
        /// Handles the result.
        /// </summary>
        /// <param name="result">The result.</param>
        protected virtual void HandleResult(T result)
        {
            Invoke(OnResult, result);
        }

    }

    /// <summary>
    /// Defines base class for all requests
    /// </summary>
    public abstract class RequestBase : IRequest
    {
        #region Fields

        private IDispatcher _dispatcher;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [use UI thread].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use UI thread]; otherwise, <c>false</c>.
        /// </value>
        public bool UseUiThread
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the logger facade.
        /// </summary>
        /// <value>The logger facade.</value>
        protected ILogger Logger
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets or sets the on error action.
        /// </summary>
        /// <value>The on error action.</value>
        public Action<ErrorDTO> OnError { get; set; }

        private bool NeedDispatch
        {
            get { return _dispatcher != null && UseUiThread && _dispatcher.CheckAccess() == false; }
        }

        #endregion

        #region Construction


        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBase"/> class.
        /// </summary>
        /// <param name="loggerFacade">The logger facade.</param>
        protected RequestBase(ILogger loggerFacade)
        {
            OnError = p => { };

            if (loggerFacade == null)
                throw new ArgumentNullException("loggerFacade");



            Logger = loggerFacade;
            UseUiThread = true;
        }

        #endregion

        #region Implementation of IRequest

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <param name="dispatcher"></param>
        public void Execute(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            Execute();
        }



        #endregion

        #region Helper methods

        /// <summary>
        /// Executes this instance.
        /// </summary>
        protected abstract void Execute();

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected virtual void HandleError(Exception ex)
        {
            Logger.Warning(ex);

            var error = ConvertToError(ex);

            if(error.Exception!=null && !ReferenceEquals(ex,error.Exception))
                Logger.Warning(error.Exception);
            else if(!string.IsNullOrWhiteSpace(error.ErrorMessage))
                Logger.Warning(error.ErrorMessage);
            
            Invoke(OnError, error);
        }


        /// <summary>
        /// Converts to error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        protected virtual ErrorDTO ConvertToError(Exception ex)
        {
            return new ErrorDTO
            {
                Exception = ex
            };
        }


        /// <summary>
        /// Invokes the specified dispatcher.
        /// </summary>
        /// <param name="action">The action.</param>
        protected void Invoke(Action action)
        {
            if (action == null) return;

            if (NeedDispatch)
                _dispatcher.BeginInvoke(action);
            else
                action();
        }


        /// <summary>
        /// Invokes the specified dispatcher.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        /// <param name="arg">The argument.</param>
        protected void Invoke<T>(Action<T> action, T arg)
        {
            if (action == null) return;

            if (NeedDispatch)
                _dispatcher.BeginInvoke(action, arg);
            else
                action(arg);
        }
        #endregion


        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {

        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
