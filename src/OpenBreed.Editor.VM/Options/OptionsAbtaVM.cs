using OpenBreed.Editor.Cfg.Managers;
using OpenBreed.Editor.Cfg.Options.ABTA;
using OpenBreed.Editor.VM.Base;
using System;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Options
{
    public class OptionsAbtaVM : BaseViewModel
    {
        #region Private Fields

        private SettingsMan settingsMan;
        private string _gameFolderPath;
        private string _gameRunFileArgs;
        private string _gameRunFilePath;

        #endregion Private Fields

        #region Public Constructors

        public OptionsAbtaVM(SettingsMan settingsMan)
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

        public string GameRunFileArgs
        {
            get { return _gameRunFileArgs; }
            set { SetProperty(ref _gameRunFileArgs, value); }
        }

        public string GameRunFilePath
        {
            get { return _gameRunFilePath; }
            set { SetProperty(ref _gameRunFilePath, value); }
        }

        public ICommand SelectGameFolderCommand { get; }

        public Func<string> OpenGameFolderDialogAction { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void SelectGameFolder()
        {
            GameFolderPath = OpenGameFolderDialogAction?.Invoke();
        }

        public void UpdateVM(ABTACfg cfg)
        {
            GameFolderPath = cfg.GameFolderPath;
            GameRunFilePath = cfg.GameRunFilePath;
            GameRunFileArgs = cfg.GameRunFileArgs;
        }

        public void UpdateCfg(ABTACfg cfg)
        {
            cfg.GameFolderPath = GameFolderPath;
            cfg.GameRunFilePath = GameRunFilePath;
            cfg.GameRunFileArgs = GameRunFileArgs;
        }

        #endregion Public Methods
    }
}