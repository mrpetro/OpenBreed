using OpenBreed.Editor.VM.Base;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Common;
using OpenBreed.Common.Maps;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Levels
{
    public class LevelEditorVM : EntryEditorBaseVM<ILevelEntry, LevelVM>
    {
        #region Private Fields

        private LevelVM _level;

        #endregion Private Fields

        #region Public Constructors

        public LevelEditorVM()
        {
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

        public LevelTileSelectorVM TileSelector { get; }
        public LevelPropSelectorVM PropSelector { get; }
        public LevelPaletteSelectorVM PaletteSelector { get; }
        public SpriteSetSelectorVM SpriteSetViewer { get; set; }

        public override string EditorName { get { return "Level Editor"; } }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Internal Methods

        internal void Connect()
        {
            PropSelector.Connect();
        }

        //public void Load(string name)
        //{
        //    CurrentLevel = Root.CreateLevel();
        //    //CurrentLevel.Load(name);

        //    TileSelector.CurrentItem = CurrentLevel.TileSets.FirstOrDefault();
        //    PropSelector.CurrentItem = CurrentLevel.PropSet;
        //    SpriteSetViewer.CurrentItem = CurrentLevel.SpriteSets.FirstOrDefault();
        //    PaletteSelector.CurrentItem = CurrentLevel.Palettes.FirstOrDefault();
        //}

        protected override void UpdateEntry(LevelVM source, ILevelEntry target)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateVM(ILevelEntry source, LevelVM target)
        {
            var model = DataProvider.GetLevel(source.Name);
            target.Name = source.Name;

            //foreach (var spriteSet in model.SpriteSets)
            //    target.AddSpriteSet(spriteSet);

            //foreach (var tileSet in model.TileSets)
            //    target.AddTileSet(tileSet);

            //if (model.PropSet != null)
            //    target.LoadPropSet(model.PropSet);

            target.Properties.Load(model.Map);
            target.Body.Load(model.Map);

            //PaletteSelector.PropertyChanged += PaletteSelector_PropertyChanged;

            //target.Restore(model.Palettes);

            //PaletteSelector.CurrentItem = target.Palettes.FirstOrDefault();
            //BodyEditor.CurrentMapBody = target.Body;
        }

        #endregion Internal Methods

        //private void PaletteSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    var paletteSelector = sender as LevelPaletteSelectorVM;

        //    switch (e.PropertyName)
        //    {
        //        case nameof(paletteSelector.CurrentItem):
        //            Root.PaletteEditor.Editable = paletteSelector.CurrentItem;
        //            Root.PaletteEditor.CurrentColorIndex = 0;

        //            foreach (var tileSet in TileSets)
        //                tileSet.Palette = paletteSelector.CurrentItem;

        //            break;
        //        default:
        //            break;
        //    }
        //}

    }
}
