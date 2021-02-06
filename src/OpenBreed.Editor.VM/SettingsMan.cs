using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools;
using OpenBreed.Editor.Cfg;
using System;
using System.IO;

namespace OpenBreed.Editor.VM
{
    public class SettingsMan : IApplicationInterface
    {
        #region Private Fields

        private const string DEFAULT_CFG_PATH = @"Resources\DefaultSettings.xml";
        private const string CFG_FILE_NAME = @"Settings.xml";

        private readonly ILogger logger;
        private readonly VariableMan variables;

        #endregion Private Fields

        #region Public Constructors

        public SettingsMan(VariableMan variables, ILogger logger)
        {
            this.variables = variables;
            this.logger = logger;
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
                    logger.Info("Settings configuration restored.");
                }
                else
                {
                    Cfg = GetDefault();
                    logger.Info("No settings file yet. Default Settings configuration restored.");
                }

                RegisterVariables();
            }
            catch (Exception ex)
            {
                logger.Error("Unable to restore settings. Reason: " + ex.Message);
            }
        }

        public void Store()
        {
            try
            {
                string cfgPath = Path.Combine(ProgramTools.AppProductDataDir, CFG_FILE_NAME);
                XmlHelper.StoreAsXml<SettingsCfg>(cfgPath, Cfg, true);

                logger.Info("Settings configuration stored.");
            }
            catch (Exception ex)
            {
                logger.Error("Unable to store settings. Reason: " + ex.Message);
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

            variables.RegisterVariable(Cfg.GetType(), Cfg, "Cfg");
            variables.RegisterVariable(typeof(string), Path.Combine(ProgramTools.AppDir, "Resources"), "App.ResourcesFolder");
        }

        #endregion Private Methods
    }
}