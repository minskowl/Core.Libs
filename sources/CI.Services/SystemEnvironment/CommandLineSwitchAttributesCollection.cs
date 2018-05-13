using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Savchin.Core;

namespace Savchin.Services.SystemEnvironment
{
    public class CommandLineSwitchAttributesCollection : IEnumerable<Tuple<CommandLineSwitchAttribute, PropertyInfo>>
    {
        private readonly string _schema;
        private readonly Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineSwitchAttributesCollection" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="schema">The schema.</param>
        public CommandLineSwitchAttributesCollection(Type type, string schema)
        {
            _type = type;
            _schema = schema;
        }

        IEnumerator<Tuple<CommandLineSwitchAttribute, PropertyInfo>> IEnumerable<Tuple<CommandLineSwitchAttribute, PropertyInfo>>.GetEnumerator()
        {
            return new CommandLineSwitchAttributesEnumerator(_type, _schema);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return new CommandLineSwitchAttributesEnumerator(_type, _schema);
        }
    }

    #region Enumerator class

    public class CommandLineSwitchAttributesEnumerator : IEnumerator<Tuple<CommandLineSwitchAttribute, PropertyInfo>>
    {
        #region Fields

        private readonly Tuple<CommandLineSwitchAttribute, PropertyInfo>[] _params;
        private int _pos;

        #endregion Fields

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineSwitchAttributesEnumerator" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="schema">The schema.</param>
        public CommandLineSwitchAttributesEnumerator(Type type, string schema)
        {
            Reset();

            var attributes = type.GetProperties().Select(GetSwitchAttributeInfo);
            _params = attributes.Where(e => e.Item1 != null && (string.IsNullOrEmpty(e.Item1.Schema) || e.Item1.Schema == schema)).ToArray();
        }

        #endregion Construction

        #region IEnumerator implementation

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            return ++_pos < _params.Length;
        }

        public void Reset()
        {
            _pos = -1;
        }

        public Tuple<CommandLineSwitchAttribute, PropertyInfo> Current
        {
            get
            {
                if (0 <= _pos && _pos < _params.Length) return _params[_pos];
                return default(Tuple<CommandLineSwitchAttribute, PropertyInfo>);
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        #endregion IEnumerator implementation

        #region Helpers

        private Tuple<CommandLineSwitchAttribute, PropertyInfo> GetSwitchAttributeInfo(PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo.GetAttribute(typeof(CommandLineSwitchAttribute), true) as CommandLineSwitchAttribute;
            return new Tuple<CommandLineSwitchAttribute, PropertyInfo>(attribute, propertyInfo);
        }

        #endregion Helpers
    }

    #endregion Enumerator class
}