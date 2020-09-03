using OpenBreed.Common;
using OpenBreed.Common.Helpers;
using OpenBreed.Common.Logging;
using OpenBreed.Editor.Cfg;
using System;
using System.IO;

namespace OpenBreed.Editor.VM
{
    public class SettingsMan
    {
        #region Private Fields

        private const string DEFAULT_CFG_PATH = @"Resources\DefaultSettings.xml";
        private const string CFG_FILE_NAME = @"Settings.xml";

        private EditorApplication application;

        #endregion Private Fields

        #region Public Constructors

        public SettingsMan(EditorApplication application)
        {
            this.application = application;
        }

        #endregion Public Constructors

        #region Public Properties

        public SettingsCfg Cfg { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Restore()
        {
            try
            {
                string cfgPath = Path.Combine(ProgramTools.AppProductDataDir, CFG_FILE_NAME);

                if (File.Exists(cfgPath))
                {
                    Cfg = XmlHelper.RestoreFromXml<SettingsCfg>(cfgPath);
                    LogMan.Instance.Success("Settings configuration restored.");
                }
                else
                {
                    Cfg = GetDefault();
                    LogMan.Instance.Success("No settings file yet. Default Settings configuration restored.");
                }

                RegisterVariables();
            }
            catch (Exception ex)
            {
                LogMan.Instance.Error("Unable to restore settings. Reason: " + ex.Message);
            }
        }

        public void Store()
        {
            try
            {
                string cfgPath = Path.Combine(ProgramTools.AppProductDataDir, CFG_FILE_NAME);
                XmlHelper.StoreAsXml<SettingsCfg>(cfgPath, Cfg, true);

                LogMan.Instance.Success("Settings configuration stored.");
            }
            catch (Exception ex)
            {
                LogMan.Instance.Error("Unable to store settings. Reason: " + ex.Message);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private SettingsCfg GetDefault()
        {
            string defaultCfgPath = Path.Combine(ProgramTools.AppDir, DEFAULT_CFG_PATH);
            return XmlHelper.RestoreFromXml<SettingsCfg>(defaultCfgPath);
        }

        private void RegisterVariables()
        {
            if (Cfg == null)
                throw new InvalidOperationException("Cfg not loaded.");

            application.Variables.RegisterVariable(Cfg.GetType(), Cfg, "Cfg");
            application.Variables.RegisterVariable(typeof(string), Path.Combine(ProgramTools.AppDir, "Resources"), "App.ResourcesFolder");
        }

        #endregion Private Methods
    }
}