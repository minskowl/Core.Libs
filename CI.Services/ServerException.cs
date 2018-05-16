using System;
using System.Runtime.Serialization;

namespace Savchin.Services
{
    [Serializable]
    public class ServerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerException"/> class.
        /// </summary>
        public ServerException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ServerException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ServerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ServerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public static class ServerExceptionExtension
    {
        public static bool IsServerException(this Exception exception)
        {
            return exception is ServerException ||
                   (exception is AggregateException && exception.InnerException is ServerException);
        }
    }
}