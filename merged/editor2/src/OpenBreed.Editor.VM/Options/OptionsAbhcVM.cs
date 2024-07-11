using OpenBreed.Editor.Cfg.Managers;
using OpenBreed.Editor.Cfg.Options.ABHC;
using OpenBreed.Editor.Cfg.Options.ABSE;
using OpenBreed.Editor.VM.Base;
using System;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Options
{
    public class OptionsAbhcVM : BaseViewModel
    {
        #region Private Fields

        private SettingsMan settingsMan;
        private string _gameFolderPath;

        #endregion Private Fields

        #region Public Constructors

        public OptionsAbhcVM(SettingsMan settingsMan)
        {
            this.settingsMan = settingsMan;

            SelectGameFolderCommand = new Command(() => SelectGameFolder());
        }

        #endregion Public Constructors

        #region Public Properties

        public string GameFolderPath
        {
            get { return _gameFolderPath; }
            set { SetProperty(ref _gameFolderPath, value); }
        }

        public ICommand SelectGameFolderCommand { get; }

        public Func<string> OpenGameFolderDialogAction { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void SelectGameFolder()
        {
            GameFolderPath = OpenGameFolderDialogAction?.Invoke();
        }

        public void UpdateVM(ABHCCfg cfg)
        {
            GameFolderPath = cfg.GameFolderPath;
        }

        public void UpdateCfg(ABHCCfg cfg)
        {
            cfg.GameFolderPath = GameFolderPath;
        }

        #endregion Public Methods
    }
}