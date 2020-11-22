using OpenBreed.Common.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenBreed.Common
{
    public class VariableMan : IApplicationInterface
    {
        #region Private Fields

        private IApplication application;
        private ILogger logger;

        private Dictionary<string, object> m_Variables = new Dictionary<string, object>();

        #endregion Private Fields

        #region Public Constructors

        public VariableMan(IApplication application)
        {
            this.application = application;
            this.logger = application.GetInterface<ILogger>();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// This recursive function registers names and values of properties in Cfg object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        public void RegisterVariable(Type type, object value, string name)
        {
            if (m_Variables.ContainsKey(name))
            {
                m_Variables[name] = value;
                return;
            }

            if (type == typeof(string) || type == typeof(decimal))
            {
                m_Variables.Add(name, value);
            }
            else
            {
                IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(value, null);

                    RegisterVariable(prop.PropertyType, propValue, name + "." + prop.Name);
                }
            }
        }

        public void UnregisterVariable(string name)
        {
            if (!m_Variables.ContainsKey(name))
                throw new Exception("Variable '{name}' not registered");

            m_Variables.Remove(name);
        }

        public string ExpandVariables(string query)
        {
            return Regex.Replace(query, @"\%[^\%]+\%", MatchEvaluator);
        }

        #endregion Public Methods

        #region Private Methods

        private string MatchEvaluator(Match match)
        {
            string varName = match.ToString().Trim(new char[] { '%' });
            object varValue = null;

            if (m_Variables.TryGetValue(varName, out varValue))
                return varValue.ToString();
            else
            {
                logger.Warning("Unknown Cfg variable: " + varName);
                return string.Empty;
            }
        }

        #endregion Private Methods
    }
}