﻿using OpenBreed.Editor.VM.Base;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Common;

namespace OpenBreed.Editor.VM.Levels
{
    public class LevelEditorVM : BaseViewModel
    {
        #region Private Fields

        private LevelVM _level;

        #endregion Private Fields

        #region Public Constructors

        public LevelEditorVM(EditorVM root)
        {
            Root = root;

            BodyEditor = new LevelBodyEditorVM(this);
            //TileSetSelector = new LevelTileSetSelectorVM(this);
            TileSelector = new LevelTileSelectorVM(this);
            PropSelector = new LevelPropSelectorVM(this);
            SpriteSetViewer = new SpriteSetSelectorVM(this);
            PaletteSelector = new LevelPaletteSelectorVM(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public LevelBodyEditorVM BodyEditor { get; }

        public LevelVM CurrentLevel
        {
            get { return _level ; }
            set { SetProperty(ref _level, value); }
        }

        public EditorVM Root { get; }
        //public LevelTileSetSelectorVM TileSetSelector { get; }
        public LevelTileSelectorVM TileSelector { get; }
        public LevelPropSelectorVM PropSelector { get; }
        public LevelPaletteSelectorVM PaletteSelector { get; }
        public SpriteSetSelectorVM SpriteSetViewer { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void TryClose()
        {

        }

        #endregion Public Methods

        #region Internal Methods

        internal void Connect()
        {
            TileSelector.Connect();
            PropSelector.Connect();
        }

        public void Load(string name)
        {
            CurrentLevel = Root.CreateLevel();
            CurrentLevel.Load(name);

            TileSelector.CurrentItem = CurrentLevel.TileSets.FirstOrDefault();
            PropSelector.CurrentItem = CurrentLevel.PropSet;
            SpriteSetViewer.CurrentItem = CurrentLevel.SpriteSets.FirstOrDefault();
            PaletteSelector.CurrentItem = CurrentLevel.Palettes.FirstOrDefault();
        }

        #endregion Internal Methods

    }
}
