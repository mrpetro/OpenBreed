using OpenBreed.Editor.Cfg.Managers;
using OpenBreed.Editor.Cfg.Options.ABSE;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Options
{
    public class OptionsVM : BaseViewModel
    {
        #region Private Fields

        private SettingsMan settingsMan;

        #endregion Private Fields

        #region Public Constructors

        public OptionsVM(SettingsMan settingsMan)
        {
            this.settingsMan = settingsMan;

            Abse = new OptionsAbseVM(settingsMan);
            Abhc = new OptionsAbhcVM(settingsMan);
            Abta = new OptionsAbtaVM(settingsMan);

            SaveCommand = new Command(() => Close(save: true));
            CancelCommand = new Command(() => Close(save: false));

            UpdateVM();
        }

        #endregion Public Constructors

        #region Public Properties

        public OptionsAbseVM Abse { get; }
        public OptionsAbhcVM Abhc { get; }
        public OptionsAbtaVM Abta { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action CloseAction { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Close(bool save)
        {
            if (save)
            {
                UpdateCfg();
            }

            CloseAction?.Invoke();
        }

        public void UpdateVM()
        {
            Abse.UpdateVM(settingsMan.Cfg.Options.ABSE);
            Abhc.UpdateVM(settingsMan.Cfg.Options.ABHC);
            Abta.UpdateVM(settingsMan.Cfg.Options.ABTA);
        }

        public void UpdateCfg()
        {
            Abse.UpdateCfg(settingsMan.Cfg.Options.ABSE);
            Abhc.UpdateCfg(settingsMan.Cfg.Options.ABHC);
            Abta.UpdateCfg(settingsMan.Cfg.Options.ABTA);

            settingsMan.Store();
        }

        #endregion Public Methods
    }
}