using System;

namespace Savchin.Services.Logging
{
    public class DebugFormatter<T> : IDebugInfoProvider
    {
        private readonly T _data;
        private readonly Func<T, object> _formatter;


        /// <summary>
        /// Initializes a new instance of the <see cref="DebugFormatter{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="formatter">The formatter.</param>
        public DebugFormatter(T data, Func<T, object> formatter)
        {
            _data = data;
            _formatter = formatter;
        }

        /// <summary>
        /// Gets the debug information.
        /// </summary>
        /// <returns></returns>
        public object GetDebugInfo()
        {
            return _formatter(_data);
        }
    }
}