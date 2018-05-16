using System;

namespace Savchin.Services.SystemEnvironment
{
    /// <summary>Implements a basic command-line switch by taking the
    /// switching name and the associated description.</summary>
    /// <remark>Only currently is implemented for properties, so all
    /// auto-switching variables should have a get/set method supplied.</remark>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CommandLineSwitchAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the schema.
        /// </summary>
        /// <value>
        /// The schema.
        /// </value>
        public string Schema { get; private set; }

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineSwitchAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="schema">The schema.</param>
        public CommandLineSwitchAttribute(string name, string description, string schema = null)
        {
            Name = name;
            Description = description;
            Schema = schema;
        }
    }

    /// <summary>
    /// This class implements an alias attribute to work in conjunction
    /// with the <see cref="CommandLineSwitchAttribute">CommandLineSwitchAttribute</see>
    /// attribute.  If the CommandLineSwitchAttribute exists, then this attribute
    /// defines an alias for it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CommandLineAliasAttribute : Attribute
    {
        #region Private Variables

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>The alias.</value>
        public string Alias { get; }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineAliasAttribute"/> class.
        /// </summary>
        /// <param name="alias">The alias.</param>
        public CommandLineAliasAttribute(string alias)
        {
            Alias = alias;
        }
        #endregion
    }

}
