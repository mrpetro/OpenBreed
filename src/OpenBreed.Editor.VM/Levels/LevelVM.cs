using OpenBreed.Common.Commands;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Common.Sources;
using System;
using System.IO;
using System.Linq;
using OpenBreed.Common.Database.Items.Levels;
using System.ComponentModel;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Common.Database.Items.Tiles;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Common.Database.Items.Props;
using OpenBreed.Common.Database.Items.Palettes;
using System.Collections.Generic;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Common.Database.Items.Sprites;

namespace OpenBreed.Editor.VM.Levels
{
    public class LevelVM : BaseViewModel
    {

        #region Private Fields

        private readonly CommandMan m_Commands;

        private bool _isModified;
        private PropSetVM _propSet;
        private SourceBase _source = null;
        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public LevelVM(EditorVM root)
        {
            Root = root;
            Body = new LevelBodyVM(this);
            Properties = new LevelPropertiesVM(this);

            Palettes = new BindingList<PaletteVM>();
            Palettes.ListChanged += (s, e) => OnPropertyChanged(nameof(Palettes));

            TileSets = new BindingList<TileSetVM>();
            TileSets.ListChanged += (s, e) => OnPropertyChanged(nameof(TileSets));

            SpriteSets = new BindingList<SpriteSetVM>();
            SpriteSets.ListChanged += (s, e) => OnPropertyChanged(nameof(SpriteSets));

            m_Commands = new CommandMan();

            PropertyChanged += MapVM_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public LevelBodyVM Body { get; }

        public CommandMan Commands { get { return m_Commands; } }

        public bool IsModified
        {
            get { return _isModified; }
            internal set { SetProperty(ref _isModified, value); }
        }

        public bool IsOpened { get { return Source != null; } }

        public LevelPropertiesVM Properties { get; }

        public PropSetVM PropSet
        {
            get { return _propSet; }
            set { SetProperty(ref _propSet, value); }
        }

        public EditorVM Root { get; }

        public SourceBase Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }

        public BindingList<PaletteVM> Palettes { get; }
        public BindingList<TileSetVM> TileSets { get; }
        public BindingList<SpriteSetVM> SpriteSets { get; }


        public int TileSize { get { return 16; } }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void AddTileSet(string name)
        {
            var newTileSet = Root.CreateTileSet();
            newTileSet.Load(name);
            TileSets.Add(newTileSet);
        }

        public void AddPalette(PaletteDef paletteDef)
        {
            var newPalette = Root.CreatePalette();
            newPalette.Load(paletteDef);
            Palettes.Add(newPalette);
        }

        public void AddSpriteSet(string name)
        {
            var newSpriteSet = Root.CreateSpriteSet();
            newSpriteSet.Load(name);
            SpriteSets.Add(newSpriteSet);
        }

        public void LoadPropSet(string name)
        {
            var propSet = Root.CreatePropSet();
            propSet.Load(name);
            PropSet = propSet;
        }
        public void Save()
        {
            //OnSaving(new EventArgs());
            //Source.Save(CurrentMap);
        }

        public void SaveAs(SourceBase newSource)
        {
            //OnSaving(new EventArgs());
            //Source = newSource;
            //Source.Save(CurrentMap);
        }

        public void TryClose()
        {
            Close();
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Close()
        {
            if (Source == null)
                throw new InvalidOperationException("There is not map loaded.");

            Source.Dispose();
            Source = null;
        }
        internal void ConnectEvents()
        {
            Body.ConnectEvents();
        }

        public void Restore(List<PaletteModel> palettes)
        {
            Palettes.RaiseListChangedEvents = false;
            Palettes.Clear();

            foreach (var palette in palettes)
            {
                var paletteVM = new PaletteVM(Root);
                paletteVM.Restore(palette);
                Palettes.Add(paletteVM);
            }

            Palettes.RaiseListChangedEvents = true;
            Palettes.ResetBindings();
        }

        internal void Load(LevelDef levelDef)
        {
            var asset = Root.DataProvider.AssetsProvider.GetAsset(levelDef.SourceRef);
            if (asset == null)
                throw new Exception("Level source error: " + levelDef.SourceRef);

            var model = Root.DataProvider.FormatMan.Load(asset, levelDef.Format) as MapModel;
            Source = asset;

            if (levelDef.TileSetRef != null)
                AddTileSet(levelDef.TileSetRef);

            if (levelDef.PropertySetRef != null)
                LoadPropSet(levelDef.PropertySetRef);

            foreach (var spriteSetRef in levelDef.SpriteSetRefs)
                AddSpriteSet(spriteSetRef);

            foreach (var paletteRef in levelDef.PaletteRefs)
            {
                var paletteDef = Root.UnitOfWork.GetPaletteDef(paletteRef);
                if (paletteDef == null)
                    throw new Exception($"No Palette definition with name '{paletteRef}' found!");

                AddPalette(paletteDef);
            }

            Properties.Load(model);
            Body.Load(model);

            if (!levelDef.PaletteRefs.Any())
            {
                Restore(model.Properties.Palettes);
                Root.LevelEditor.PaletteSelector.CurrentItem = Palettes.FirstOrDefault();
            }

            Root.LevelEditor.BodyEditor.CurrentMapBody = Body;
        }

        #endregion Internal Methods

        #region Private Methods

        private void MapVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Source):
                    UpdateTitle();
                    break;
                default:
                    break;
            }
        }
        private string MarkNameIfModified()
        {
            if (IsModified)
                return Source.Name + "*";
            else
                return Source.Name;
        }

        private void UpdateTitle()
        {
            if (Source == null)
                Title = "<No map>";
            else
                Title = MarkNameIfModified();
        }

        #endregion Private Methods

    }
}