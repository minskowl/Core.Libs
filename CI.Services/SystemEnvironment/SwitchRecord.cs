using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Savchin.Services.SystemEnvironment
{
    /// <summary>
    /// The SwitchRecord is stored within the parser's collection of registered
    /// switches.  This class is private to the outside world.
    /// </summary>
    public class SwitchRecord
    {
        #region Private Variables

        private readonly Type _switchType;
        private readonly List<string> _aliases = new List<string>();


        // The following advanced functions allow for callbacks to be
        // made to manipulate the associated data type.

        #endregion

        #region Public Properties
        public object Value
        {
            get { return ReadValue ?? InternalValue; }
        }

        /// <summary>
        /// Gets the internal value.
        /// </summary>
        /// <value>The internal value.</value>
        public object InternalValue { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type
        {
            get { return _switchType; }
        }

        /// <summary>
        /// Gets the aliases.
        /// </summary>
        /// <value>The aliases.</value>
        public string[] Aliases
        {
            get { return _aliases.ToArray(); }
        }


        /// <summary>
        /// Sets the set method.
        /// </summary>
        /// <value>The set method.</value>
        public MethodInfo SetMethod { private get; set; }

        /// <summary>
        /// Sets the get method.
        /// </summary>
        /// <value>The get method.</value>
        public MethodInfo GetMethod { private get; set; }

        /// <summary>
        /// Sets the property owner.
        /// </summary>
        /// <value>The property owner.</value>
        public object PropertyOwner { private get; set; }

        private Regex _regex;
        /// <summary>
        /// Gets the regex.
        /// </summary>
        /// <value>The regex.</value>
        public Regex Regex
        {
            get
            {
                if (_regex == null)
                    _regex = new Regex(BuildPattern(),
                                      RegexOptions.ExplicitCapture |
                                      RegexOptions.IgnoreCase |
                                      RegexOptions.Compiled);
                return _regex;
            }
        }

        public object ReadValue
        {
            get
            {
                object o = null;
                if (PropertyOwner != null && GetMethod != null)
                    o = GetMethod.Invoke(PropertyOwner, null);
                return o;
            }
        }

        public string[] Enumerations
        {
            get { return _switchType.IsEnum ? Enum.GetNames(_switchType) : null; }
        }



        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchRecord"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public SwitchRecord(string name, string description)
            : this(name, description, typeof(bool))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchRecord"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="type">The type.</param>
        public SwitchRecord(string name, string description, Type type)
        {
            if (type != typeof(bool) && type != typeof(string) && type != typeof(int) && !type.IsEnum)
                throw new ArgumentException("Currently only Ints, Bool and Strings are supported");

            _switchType = type;
            Name = name;
            Description = description;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        public void AddAlias(string alias)
        {
            _aliases.Add(alias);
        }

        /// <summary>
        /// Notifies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Notify(object value)
        {
            if (PropertyOwner != null && SetMethod != null)
            {
                var parameters = new object[1];
                parameters[0] = value;
                SetMethod.Invoke(PropertyOwner, parameters);
            }
            InternalValue = value;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Private Utility Functions

        private string BuildPattern()
        {
            string matchString = Name;

            if (Aliases != null && Aliases.Length > 0)
                foreach (string s in Aliases)
                    matchString += "|" + s;

            string strPatternStart = @"(\s|^)(?<match>(-{1,2}|/)(";
            string strPatternEnd;  // To be defined below.

            // The common suffix ensures that the switches are followed by
            // a white-space OR the end of the string.  This will stop
            // switches such as /help matching /helpme
            //
            string strCommonSuffix = @"(?=(\s|$))";

            if (Type == typeof(bool))
                strPatternEnd = @")(?<value>(\+|-){0,1}))";
            else if (Type == typeof(string))
                strPatternEnd = @")(?::|\s+))((?:"")(?<value>[^""]+)(?:"")|(?<value>\S+))";
            else if (Type == typeof(int))
                strPatternEnd = @")(?::|\s+))((?<value>(-|\+)[0-9]+)|(?<value>[0-9]+))";
            else if (Type.IsEnum)
            {
                string[] enumNames = Enumerations;
                string e_str = enumNames[0];
                for (int e = 1; e < enumNames.Length; e++)
                    e_str += "|" + enumNames[e];
                strPatternEnd = @")(?::|\s+))(?<value>" + e_str + @")";
            }
            else
                throw new ArgumentException();

            // Set the internal regular expression pattern.
            return strPatternStart + matchString + strPatternEnd + strCommonSuffix;


        }

        #endregion
    }
}
