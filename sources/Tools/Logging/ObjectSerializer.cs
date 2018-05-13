using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Savchin.Logging
{
    public class ObjectSerializer
    {
        private readonly List<Func<Type, bool>> _matchers = new List<Func<Type, bool>>();

        /// <summary>
        /// Gets or sets the current.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static ObjectSerializer Current { get; set; }

        #region Construction

        /// <summary>
        /// Initializes the <see cref="ObjectSerializer"/> class.
        /// </summary>
        static ObjectSerializer()
        {
            Current = new ObjectSerializer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectSerializer"/> class.
        /// </summary>
        public ObjectSerializer()
        {
            AddMatcher(
                t => string.Equals(t.Namespace, "System", StringComparison.Ordinal) &&
                !t.FullName.StartsWith("System.Action", StringComparison.Ordinal));
        }

        #endregion Construction

        /// <summary>
        /// Adds the matcher.
        /// </summary>
        /// <param name="matcher">The matcher.</param>
        public void AddMatcher(Func<Type, bool> matcher)
        {
            _matchers.Add(matcher);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="value">The object.</param>
        /// <returns></returns>
        public string Serialize(object value)
        {
            if (value == null) return "null";

            var serializer = value as IDebugInfoProvider;
            if (serializer != null)
                return Serialize(serializer.GetDebugInfo());

            return IsSerializable(value.GetType())
                ? JsonConvert.SerializeObject(value)
                : value.ToString();
        }

        private bool IsSerializable(Type type)
        {
            return _matchers.Any(match => match(type));
        }
    }
}
