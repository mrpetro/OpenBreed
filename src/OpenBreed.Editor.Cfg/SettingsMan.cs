using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using OpenBreed.Common;
using OpenBreed.Common.Logging;

namespace OpenBreed.Editor.Cfg
{
    public class SettingsMan
    {
        private const string DEFAULT_CFG_PATH = @"Resources\DefaultSettings.xml";
        private const string CFG_PATH = @"Settings.xml";

        private Dictionary<string, object> m_Variables = new Dictionary<string,object>();

        private SettingsCfg m_Cfg;

        public SettingsCfg Cfg { get { return m_Cfg; } }

        public SettingsMan()
        {
        }

        private SettingsCfg GetDefault()
        {
            string defaultCfgPath = Path.Combine(ProgramTools.AppDir, DEFAULT_CFG_PATH);
            return Tools.RestoreFromXml<SettingsCfg>(defaultCfgPath);
        }

        private void RegisterVariables()
        {
            if (Cfg == null)
                throw new InvalidOperationException("Cfg not loaded.");

            m_Variables.Clear();

            RegisterVariable(Cfg.GetType(), Cfg, "Cfg");
            RegisterVariable(typeof(string), Path.Combine(ProgramTools.AppDir, "Resources"), "App.ResourcesFolder");
        }

        /// <summary>
        /// This recursive function registers names and values of properties in Cfg object 
        /// </summary>
        /// <param name="varType"></param>
        /// <param name="varValue"></param>
        /// <param name="varName"></param>
        private void RegisterVariable( Type varType, object varValue, string varName)
        {
            if (varType == typeof(string) || varType == typeof(decimal))
            {
                m_Variables.Add(varName, varValue);
            }
            else
            {
                IList<PropertyInfo> props = new List<PropertyInfo>(varType.GetProperties());

                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(varValue, null);

                    RegisterVariable(prop.PropertyType, propValue, varName + "." + prop.Name);
                }
            }
        }

        public void Restore()
        {
            try
            {
                string cfgPath = Path.Combine(ProgramTools.AppDataDir, CFG_PATH);

                if (File.Exists(cfgPath))
                {
                    m_Cfg = Tools.RestoreFromXml<SettingsCfg>(cfgPath);
                    LogMan.Instance.LogSuccess("Settings configuration restored.");
                }
                else
                {
                    m_Cfg = GetDefault();
                    LogMan.Instance.LogSuccess("No settings file yet. Default Settings configuration restored.");
                }

                RegisterVariables();
            }
            catch (Exception ex)
            {
                LogMan.Instance.LogError("Unable to restore settings. Reason: " + ex.Message);
            }
        }

        public void Store()
        {
            try
            {
                string cfgPath = Path.Combine(ProgramTools.AppDataDir, CFG_PATH);
                Tools.StoreAsXml<SettingsCfg>(cfgPath,m_Cfg, true);

                LogMan.Instance.LogSuccess("Settings configuration storred.");
            }
            catch (Exception ex)
            {
                LogMan.Instance.LogError("Unable to store settings. Reason: " + ex.Message);
            }
        }

        private string MatchEvaluator(Match match)
        {
            string varName = match.ToString().Trim(new char [] { '%' });
            object varValue = null;

            if (m_Variables.TryGetValue(varName, out varValue))
                return varValue.ToString();
            else
            {
                LogMan.Instance.LogWarning("Unknown Cfg variable: " + varName);
                return string.Empty;
            }
        }

        public string ExpandVariables(string query)
        {
            return Regex.Replace(query, @"\%[^\%]+\%", MatchEvaluator);
        }
    }
}
