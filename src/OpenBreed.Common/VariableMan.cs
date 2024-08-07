﻿using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenBreed.Common
{
    public class VariableMan : IVariableMan
    {
        #region Private Fields

        private readonly ILogger logger;
        private readonly Dictionary<string, object> variables = new Dictionary<string, object>();

        #endregion Private Fields

        #region Public Constructors

        public VariableMan(ILogger logger)
        {
            this.logger = logger;
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
            if (variables.ContainsKey(name))
            {
                variables[name] = value;
                return;
            }

            if (type == typeof(string) || type == typeof(decimal))
            {
                variables.Add(name, value);
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
            if (!variables.ContainsKey(name))
                throw new Exception("Variable '{name}' not registered");

            variables.Remove(name);
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

            if (!variables.TryGetValue(varName, out varValue))
            {
                logger.LogWarning("Unknown Cfg variable: " + varName);
                return string.Empty;
            }

            if(varValue is null)
            {
                logger.LogError($"Cfg variable '{varName}' is unset.");
                return string.Empty;
            }

            return varValue.ToString();
        }

        #endregion Private Methods
    }
}