using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;
using Savchin.Core;
using Savchin.Services.Localization;

namespace Savchin.Services
{
    [DataContract]
    public class ErrorDTO
    {
        #region Constants

        private const string ConnectionLostKey = "ConnectionLost_Text";

        #endregion Constants

        #region DTO Properties

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        [DataMember]
        [XmlAttribute]
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        [DataMember]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status.
        /// </summary>
        /// <value>The HTTP status.</value>
        [DataMember]
        [XmlAttribute]
        public int HttpStatus { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        [DataMember]
        [XmlAttribute]
        public int StatusCode { get; set; }

        #endregion DTO Properties

        #region Properties

        private Exception _exception;
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        [XmlIgnore]
        public Exception Exception
        {
            get
            {
                return _exception;
            }
            set
            {
                _exception = value;
                _exceptionData = IsSerializable(value) ? TypeSerializer<Exception>.ToBinary(value) : null;
               ErrorMessage = value?.Message;
            }
        }

        private byte[] _exceptionData;
        /// <summary>
        /// Gets or sets the exception data.
        /// </summary>
        /// <value>
        /// The exception data.
        /// </value>
        public byte[] ExceptionData
        {
            get { return _exceptionData; }
            set
            {
                _exceptionData = value;
                _exception = value == null || !value.Any() ? null : TypeSerializer<Exception>.FromBinary(_exceptionData);
            }
        }

        #endregion Properties

        #region Public methods

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="localizationRegistry">The localization registry.</param>
        /// <returns>Short message describing error</returns>
        public string BuildMessage(ILocalizationRegistry localizationRegistry)
        {
            return Exception == null ? ErrorMessage : GetTitleFromException(Exception, localizationRegistry);
        }

        /// <summary>
        /// Builds the details.
        /// </summary>
        /// <returns>
        /// Detailed description of error
        /// </returns>
        public string BuildDetails()
        {
            return Exception?.Message ?? ErrorMessage;
        }

        /// <summary>
        /// Returns a <see cref="System.string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            if (ErrorCode > 0 || HttpStatus > 0)
                builder.AppendFormat("ErrorCode={0} HttpStatus={1} {2}", ErrorCode, HttpStatus, Environment.NewLine);
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
                builder.AppendLine(ErrorMessage);
            if (Exception != null)
                builder.AppendLine(Exception.ToString());

            return builder.ToString();
        }

        #endregion Public methods

        #region Helper methods

        private static bool IsSerializable(Exception exception)
        {
            if (exception == null)
                return false;

            var isSerializable = exception.GetType().IsSerializable;
            if (!isSerializable)
                return false;

            return exception.InnerException == null || IsSerializable(exception.InnerException);
        }

        private string GetTitleFromException(Exception exception, ILocalizationRegistry localizationRegistry)
        {
            var informativeException = ExtractInformativeException(exception);
            if (informativeException == null) return ErrorMessage;

            var communicationException = informativeException as CommunicationException;
            if (communicationException != null)
            {
                return localizationRegistry.CurrentProvider.GetString(ConnectionLostKey);
            }

            var fileNotFoundException = informativeException as FileNotFoundException;
            if (fileNotFoundException != null)
            {
                return $"{informativeException.Message} ({fileNotFoundException.FileName})";
            }

            return informativeException.Message;
        }

        private static Exception ExtractInformativeException(Exception ex)
        {
            if (ex is FileNotFoundException)
            {
                return ex;
            }

            if (ex is ServerException)
            {
                return ex.InnerException;
            }

            if (ex.InnerException is WebException)
            {
                return ex.InnerException;
            }

            if (ex.InnerException is TimeoutException)
            {
                return ex.InnerException;
            }
            
            return null;
        }

        #endregion Helper methods
    }
}
