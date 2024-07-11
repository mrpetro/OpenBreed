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
    public class OptionsAbseVM : BaseViewModel
    {
        #region Private Fields

        private SettingsMan settingsMan;
        private string _gameFolderPath;

        #endregion Private Fields

        #region Public Constructors

        public OptionsAbseVM(SettingsMan settingsMan)
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

        public void UpdateVM(ABSECfg cfg)
        {
            GameFolderPath = cfg.GameFolderPath;
        }

        public void UpdateCfg(ABSECfg cfg)
        {
            cfg.GameFolderPath = GameFolderPath;
        }

        #endregion Public Methods
    }
}