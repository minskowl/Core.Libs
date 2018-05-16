using System.Collections.Generic;

namespace Savchin.ComponentModel
{
    /// <summary>
    /// This is helper class used to define boolean lists 
    /// </summary>
    public class ItemModel<T> : ObjectBase
    {
        #region Constants
        private static readonly string ValuePropertyName = nameof(Value);

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemModel&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public ItemModel(string name, T value)
        {
            _name = name;
            _value = value;
        }

        #endregion

        #region Properties

        private string _name;

        /// <summary>
        /// Gets or sets the item name.
        /// </summary>
        /// <value>The item name.</value>
        public string Name
        {
            get { return _name; }
            set
            {
                Set(ref _name, value);
            }
        }

        private T _value;

        /// <summary>
        /// Gets or sets the item value.
        /// </summary>
        /// <value>The item value.</value>
        public T Value
        {
            get { return _value; }
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value))
                    return;

                _value = value;
                OnPropertyChanged(ValuePropertyName);
            }
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
