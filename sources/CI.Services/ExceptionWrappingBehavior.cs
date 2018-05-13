using System;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;

namespace Savchin.Services
{
    public static class ExceptionWrappingBehavior
    {
        public static Task<TResult> WithExceptionWrapping<TResult>(this Task<TResult> wcfCall, Action<Exception> precondition = null)
        {
            return WrapException(() => wcfCall, precondition);
        }

        private static Task<TResult> WrapException<TResult>(Func<Task<TResult>> wcfCall, Action<Exception> precondition = null)
        {
            return wcfCall().ContinueWith(task =>
            {
                if (task.Exception == null) return task;

                var exception = task.Exception.GetBaseException();

                if (exception is FaultException)
                    return task;

                precondition?.Invoke(exception);

                var webException = exception as WebException;
                if (webException != null)
                    throw new ServerException($"Web Exception {webException.Status}", exception);

                var socketException = exception as SocketException;
                if (socketException != null)
                    throw new ServerException($"Socket Exception {socketException.SocketErrorCode}", exception);

                if (exception is TimeoutException)
                    throw new ServerException("Timeout", exception);

                if (exception is MessageSecurityException)
                    throw new ServerException($"Message Security Exception {exception.Message}", exception);

                if (exception is CommunicationException)
                    throw new ServerException($"Communication Exception {exception.GetType()}", exception);

                return task;
            }, TaskContinuationOptions.AttachedToParent).Unwrap();
        }
    }
 
}