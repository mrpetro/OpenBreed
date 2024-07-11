using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Editor.Cfg;
using System;
using System.IO;

namespace OpenBreed.Editor.Cfg.Managers
{
    public class SettingsMan
    {
        #region Private Fields

        private const string DEFAULT_CFG_PATH = @"Resources\DefaultSettings.xml";
        private const string CFG_FILE_NAME = @"Settings.xml";

        private readonly ILogger logger;
        private readonly IVariableMan variables;

        #endregion Private Fields

        #region Public Constructors

        public SettingsMan(IVariableMan variables, ILogger logger)
        {
            this.variables = variables;
            this.logger = logger;

            Restore();
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
                    logger.LogInformation("Settings configuration restored.");
                }
                else
                {
                    Cfg = GetDefault();
                    logger.LogInformation("No settings file yet. Default Settings configuration restored.");
                }

                RegisterVariables();
            }
            catch (Exception ex)
            {
                logger.LogError("Unable to restore settings. Reason: " + ex.Message);
            }
        }

        public void Store()
        {
            try
            {
                string cfgPath = Path.Combine(ProgramTools.AppProductDataDir, CFG_FILE_NAME);
                XmlHelper.StoreAsXml<SettingsCfg>(cfgPath, Cfg, true);

                logger.LogInformation("Settings configuration stored.");
            }
            catch (Exception ex)
            {
                logger.LogError("Unable to store settings. Reason: " + ex.Message);
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
            if (Cfg is null)
            {
                throw new InvalidOperationException("Cfg not loaded.");
            }

            variables.RegisterVariable(Cfg.GetType(), Cfg, "Cfg");
            variables.RegisterVariable(typeof(string), Path.Combine(ProgramTools.AppDir, "Resources"), "App.ResourcesFolder");
        }

        #endregion Private Methods
    }
}