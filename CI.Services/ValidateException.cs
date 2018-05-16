using System;
using System.Runtime.Serialization;
using Savchin.Services.Localization;

namespace Savchin.Services
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [Serializable]
    public class ValidateException : LocalizableException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateException" /> class.
        /// </summary>
        /// <param name="localizationKey">The localization key.</param>
        public ValidateException(string localizationKey) : base(localizationKey)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateException" /> class.
        /// </summary>
        /// <param name="localizationKey">The localization key.</param>
        /// <param name="arguments">The arguments.</param>
        public ValidateException(string localizationKey, params object[] arguments) : base(localizationKey, arguments)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateException" /> class.
        /// </summary>
        /// <param name="localizationKey">The localization key.</param>
        /// <param name="arg">The argument.</param>
        public ValidateException(string localizationKey, object arg) : base(localizationKey, arg)
        {


        }



        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected ValidateException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
