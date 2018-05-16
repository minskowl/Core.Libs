using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Savchin.Collection.Generic;

namespace Savchin.Services.SystemEnvironment
{
    /// <summary>Implementation of a command-line parsing class.  Is capable of
    /// having switches registered with it directly or can examine a registered
    /// class for any properties with the appropriate attributes appended to
    /// them.</summary>
    public class CommandLineParser
    {
        #region Private Variables

        readonly Regex _regexUnhadledSwitches = new Regex(@"(\s|^)(?<match>(-{1,2}|/)(.+?))(?=(\s|$))", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private readonly string _commandLine;
        private string _workingString = "";
        private string _applicationName = "";
        private string[] _splitParameters;

        private readonly Dictionary<string, SwitchRecord> _switches = new Dictionary<string, SwitchRecord>();
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public string ApplicationName
        {
            get { return _applicationName; }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public string[] Parameters
        {
            get { return _splitParameters; }
        }

        /// <summary>
        /// Gets the switches.
        /// </summary>
        /// <value>The switches.</value>
        public SwitchRecord[] Switches
        {
            get
            {
                SwitchRecord[] result = new SwitchRecord[_switches.Values.Count];
                _switches.Values.CopyTo(result, 0);
                return result;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        /// <value></value>
        public object this[string name]
        {
            get
            {
                if (_switches.ContainsKey(name))
                    return _switches[name].Value;
                return null;
            }
        }

        /// <summary>This function returns a list of the unhandled switches
        /// that the parser has seen, but not processed.</summary>
        /// <remark>The unhandled switches are not removed from the remainder
        /// of the command-line.</remark>
        public string[] UnhandledSwitches
        {
            get
            {
                MatchCollection matchCollection = _regexUnhadledSwitches.Matches(_workingString);
                List<string> result = new List<string>(matchCollection.Count);
                foreach (Match match in matchCollection)
                {
                    result.Add(match.Groups["match"].Value);
                }

                return result.ToArray();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds the switch.
        /// </summary>
        /// <param name="switchRecord">The switch record.</param>
        public void AddSwitch(SwitchRecord switchRecord)
        {
            _switches.Add(switchRecord.Name, switchRecord);
        }

        /// <summary>
        /// Adds the switch.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public SwitchRecord AddSwitch(string name, string description)
        {
            var result = new SwitchRecord(name, description);
            _switches.Add(name, result);
            return result;
        }

        /// <summary>
        /// Adds the switch.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <param name="description">The description.</param>
        public void AddSwitch(string[] names, string description)
        {
            var rec = new SwitchRecord(names[0], description);
            for (var s = 1; s < names.Length; s++)
                rec.AddAlias(names[s]);
            _switches.Add(names[0], rec);
        }

        /// <summary>
        /// Parses this instance.
        /// </summary>
        /// <returns></returns>
        public bool Parse()
        {
            ExtractApplicationName();

            // Remove switches and associated info.
            foreach (SwitchRecord switchRecord in _switches.Values)
            {
                ProcessSwitch(switchRecord);
            }

            // Split parameters.
            SplitParameters();

            return true;
        }

        public string GetHelp(Type type, string schema = null)
        {

            var builder = new StringBuilder();

            new CommandLineSwitchAttributesCollection(type, schema)
                .ForEach(tuple => builder.AppendFormat("-{0,-20} - {1} {2}", tuple.Item1.Name, tuple.Item1.Description, Environment.NewLine));

            return builder.ToString();

        }

        /// <summary>
        /// Internals the value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public object InternalValue(string name)
        {
            if (_switches.ContainsKey(name))
                return _switches[name].InternalValue;
            return null;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParser"/> class.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        public CommandLineParser(string commandLine)
        {
            _commandLine = commandLine;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParser" /> class.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        /// <param name="obj">The class for auto attributes.</param>
        /// <param name="schema">The schema.</param>
        public CommandLineParser(string commandLine, object obj, string schema = null)
        {
            _commandLine = commandLine;

            var type = obj.GetType();

            new CommandLineSwitchAttributesCollection(type, schema)
                .ForEach(tuple => _switches.Add(tuple.Item1.Name, CreateSwitchRecord(obj, tuple.Item2, tuple.Item1)));
        }

        private SwitchRecord CreateSwitchRecord(object obj, MemberInfo member, CommandLineSwitchAttribute switchAttribute)
        {
            SwitchRecord rec;
            if (member is PropertyInfo)
            {
                var pi = (PropertyInfo)member;

                rec = new SwitchRecord(switchAttribute.Name,
                                       switchAttribute.Description,
                                       pi.PropertyType)
                {
                    SetMethod = pi.GetSetMethod(),
                    GetMethod = pi.GetGetMethod(),
                    PropertyOwner = obj
                };

                // Map in the Get/Set methods.
            }
            else
            {
                throw new NotImplementedException(member.GetType().FullName);
            }

            foreach (CommandLineAliasAttribute aliasAttrib in member.GetCustomAttributes(typeof(CommandLineAliasAttribute), false))
            {
                rec.AddAlias(aliasAttrib.Alias);
            }
            return rec;
        }

        #endregion

        #region Private Utility Functions

        private void ExtractApplicationName()
        {
            var r = new Regex(@"^(?<commandLine>(""[^""]+""|(\S)+))(?<remainder>.+)", RegexOptions.ExplicitCapture);
            Match m = r.Match(_commandLine);
            if (m == null || m.Groups["commandLine"] == null) return;

            _applicationName = m.Groups["commandLine"].Value;
            _workingString = m.Groups["remainder"].Value;
        }

        private void SplitParameters()
        {
            // Populate the split parameters array with the remaining parameters.
            // Note that if quotes are used, the quotes are removed.
            // e.g.   one two three "four five six"
            //						0 - one
            //						1 - two
            //						2 - three
            //						3 - four five six
            // (e.g. 3 is not in quotes).
            var r = new Regex(@"((\s*(""(?<param>.+?)""|(?<param>\S+))))", RegexOptions.ExplicitCapture);
            var m = r.Matches(_workingString);

            if (m != null)
            {
                _splitParameters = new string[m.Count];
                for (int i = 0; i < m.Count; i++)
                    _splitParameters[i] = m[i].Groups["param"].Value;
            }
        }



        private void ProcessSwitch(SwitchRecord switchRecord)
        {
            MatchCollection matchCollection = switchRecord.Regex.Matches(_workingString);

            foreach (Match match in matchCollection)
            {
                string value = null;
                if (match.Groups["value"] != null)
                    value = match.Groups["value"].Value;

                if (switchRecord.Type == typeof(bool))
                {
                    bool state = true;
                    // The value string may indicate what value we want.
                    if (value != null)
                    {
                        switch (value)
                        {
                            case "+":
                                state = true;
                                break;
                            case "-":
                                state = false;
                                break;
                            case "":
                                if (switchRecord.ReadValue != null)
                                    state = !(bool)switchRecord.ReadValue;
                                break;
                        }
                    }
                    switchRecord.Notify(state);
                    break;
                }
                if (switchRecord.Type == typeof(string))
                    switchRecord.Notify(value);
                else if (switchRecord.Type == typeof(int))
                    switchRecord.Notify(int.Parse(value));
                else if (switchRecord.Type.IsEnum)
                    switchRecord.Notify(Enum.Parse(switchRecord.Type, value, true));
            }

            _workingString = switchRecord.Regex.Replace(_workingString, " ");
        }

        #endregion
    }
}
