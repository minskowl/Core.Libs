using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Util;

namespace Savchin.Logging
{
    public class LoggerStringFormat
    {
        #region Fields

        private static readonly Type DeclaringType = typeof(LoggerStringFormat);
        private readonly IFormatProvider _provider;
        private readonly string _format;
        private readonly object[] _args;

        #endregion Fields

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerStringFormat"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public LoggerStringFormat(IFormatProvider provider, string format, params object[] args)
        {
            _provider = provider;
            _format = format;
            _args = args;
        }

        #endregion Construction

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return StringFormat(_provider, _format, _args);
        }

        #region Helpers

        private static string StringFormat(IFormatProvider provider, string format, params object[] args)
        {
            try
            {
                if (format == null || args == null)
                    return format;

                return string.Format(provider, format, args.Select(Convert).ToArray());
            }
            catch (Exception ex)
            {
                LogLog.Warn(DeclaringType, "Exception while rendering format [" + format + "]", ex);
                return StringFormatError(ex, format, args);
            }
        }

        private static object Convert(object o)
        {
            return ObjectSerializer.Current?.Serialize(o) ?? string.Empty;
        }

        private static string StringFormatError(Exception formatException, string format, object[] args)
        {
            try
            {
                var buffer = new StringBuilder("<log4net.Error>");
                if (formatException != null)
                    buffer.Append("Exception during StringFormat: ").Append(formatException.Message);
                else
                    buffer.Append("Exception during StringFormat");
                buffer.Append(" <format>").Append(format).Append("</format>");
                buffer.Append("<args>");
                RenderArray(args, buffer);
                buffer.Append("</args>");
                buffer.Append("</log4net.Error>");
                return buffer.ToString();
            }
            catch (Exception ex)
            {
                LogLog.Error(DeclaringType, "INTERNAL ERROR during StringFormat error handling", ex);
                return "<log4net.Error>Exception during StringFormat. See Internal Log.</log4net.Error>";
            }
        }

        private static void RenderArray(Array array, StringBuilder buffer)
        {
            if (array == null)
                buffer.Append(SystemInfo.NullText);
            else if (array.Rank != 1)
            {
                buffer.Append(array);
            }
            else
            {
                buffer.Append("{");
                var length = array.Length;
                if (length > 0)
                {
                    RenderObject(array.GetValue(0), buffer);
                    for (var index = 1; index < length; ++index)
                    {
                        buffer.Append(", ");
                        RenderObject(array.GetValue(index), buffer);
                    }
                }
                buffer.Append("}");
            }
        }

        private static void RenderObject(object obj, StringBuilder buffer)
        {
            if (obj == null)
            {
                buffer.Append(SystemInfo.NullText);
            }
            else
            {
                try
                {
                    buffer.Append(obj);
                }
                catch (Exception ex)
                {
                    buffer.Append("<Exception: ").Append(ex.Message).Append(">");
                }
            }
        }

        #endregion Helpers
    }
}
