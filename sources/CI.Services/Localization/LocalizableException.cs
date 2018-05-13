using System;
using System.Runtime.Serialization;

namespace Savchin.Services.Localization
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [Serializable]
    public class LocalizableException : Exception
    {
        public string LocalizationKey { get; }
        public object[] Arguments { get; }

        public override string Message {
            get
            {
                if (Arguments == null)
                    return LocalizationHelper.GetString(LocalizationKey);
                if(Arguments.Length==1)
                    return LocalizationHelper.GetString(LocalizationKey,Arguments[0]);

                return LocalizationHelper.GetString(LocalizationKey, Arguments);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableException"/> class.
        /// </summary>
        /// <param name="localizationKey">The localization key.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
        protected LocalizableException(string localizationKey)
        {
            LocalizationKey = localizationKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableException"/> class.
        /// </summary>
        /// <param name="localizationKey">The localization key.</param>
        /// <param name="arguments">The arguments.</param>
        protected internal LocalizableException(string localizationKey, params object[] arguments) 
        {
            LocalizationKey = localizationKey;
            Arguments = arguments;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableException"/> class.
        /// </summary>
        /// <param name="localizationKey">The localization key.</param>
        /// <param name="arg">The argument.</param>
        protected LocalizableException(string localizationKey,object arg) : base(LocalizationHelper.GetString(localizationKey, arg))
        {
            LocalizationKey = localizationKey;
            Arguments = new[] { arg }; 

        }

        protected LocalizableException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            LocalizationKey = info.GetString("LocalizationKey");
            Arguments=info.GetValue("Args", typeof(object[])) as object[];
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("LocalizationKey", LocalizationKey, typeof(String));
            info.AddValue("Args", Arguments, typeof(object[]));
        }
    }   
}
