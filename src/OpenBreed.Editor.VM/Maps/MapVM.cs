using OpenBreed.Common.Commands;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Common.Assets;
using System;
using System.IO;
using System.Linq;
using System.ComponentModel;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Actions;
using System.Collections.Generic;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Common.Tiles;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Actions;
using OpenBreed.Common;
using System.Drawing;
using OpenBreed.Common.Data;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapVM : EditableEntryVM
    {

        #region Private Fields

        private MapModel _model;
        private ActionSetVM _actionSet;
        private bool _isModified;
        private AssetBase _source = null;
        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public MapVM()
        {
            Layout = new MapLayoutVM(this);
            Properties = new LevelPropertiesVM(this);

            TileSets = new BindingList<TileSetVM>();
            TileSets.ListChanged += (s, e) => OnPropertyChanged(nameof(TileSets));

            Palettes = new BindingList<PaletteVM>();
            Palettes.ListChanged += (s, e) => OnPropertyChanged(nameof(Palettes));

            SpriteSets = new BindingList<SpriteSetVM>();
            SpriteSets.ListChanged += (s, e) => OnPropertyChanged(nameof(SpriteSets));

            PropertyChanged += MapVM_PropertyChanged;

            Layout.PropertyChanged += (s, e) => OnPropertyChanged(nameof(Layout));
        }

        #endregion Public Constructors

        #region Public Properties

        public ActionSetVM ActionSet
        {
            get { return _actionSet; }
            set { SetProperty(ref _actionSet, value); }
        }

        public bool IsModified
        {
            get { return _isModified; }
            internal set { SetProperty(ref _isModified, value); }
        }

        public bool IsOpened { get { return Source != null; } }

        public MapLayoutVM Layout { get; }

        public BindingList<PaletteVM> Palettes { get; }
        public LevelPropertiesVM Properties { get; }
        public AssetBase Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }
        public BindingList<SpriteSetVM> SpriteSets { get; }
        public BindingList<TileSetVM> TileSets { get; }

        public int TileSize { get { return 16; } }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public Point GetIndexCoords(Point point)
        {
            return new Point(point.X / TileSize, point.Y / TileSize);
        }

        public void Save()
        {
            //OnSaving(new EventArgs());
            //Source.Save(CurrentMap);
        }

        public void SaveAs(AssetBase newSource)
        {
            //OnSaving(new EventArgs());
            //Source = newSource;
            //Source.Save(CurrentMap);
        }

        public void SetPalettes(List<PaletteModel> palettes)
        {
            Palettes.UpdateAfter(() =>
            {
                Palettes.Clear();

                foreach (var palette in palettes)
                {
                    var paletteVM = new PaletteVM();
                    paletteVM.FromModel(palette);
                    Palettes.Add(paletteVM);
                }
            });
        }

        public void SetTileSets(List<TileSetModel> tileSets)
        {
            TileSets.UpdateAfter(() =>
            {
                TileSets.Clear();

                foreach (var tileSet in tileSets)
                {
                    var tileSetVM = new TileSetVM();
                    tileSetVM.FromModel(tileSet);
                    TileSets.Add(tileSetVM);
                }
            });
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
            Layout.ConnectEvents();
        }

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);

            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var mapEntry = entry as IMapEntry;

            if (mapEntry.TileSetRef != null)
            {
                var tileSet = dataProvider.TileSets.GetTileSet(mapEntry.TileSetRef);
                SetTileSets(new List<TileSetModel>( new TileSetModel[] { tileSet }));
            }

            if (mapEntry.ActionSetRef != null)
            {
                var actionSet = dataProvider.ActionSets.GetActionSet(mapEntry.ActionSetRef);
                if (actionSet != null)
                    SetActionSet(actionSet);
            }

            foreach (var spriteSetRef in mapEntry.SpriteSetRefs)
            {
                var spriteSet = dataProvider.SpriteSets.GetSpriteSet(spriteSetRef);
                //AddSpriteSet(spriteSet);
            }

            _model = dataProvider.Maps.Load(entry.Id);

            Properties.Load(_model);

            Layout.FromMap(_model);

            var palettes = new List<PaletteModel>();

            if (mapEntry.PaletteRefs.Any())
            {
                foreach (var paletteRef in mapEntry.PaletteRefs)
                    palettes.Add(dataProvider.Palettes.GetPalette(paletteRef));
            }
            else
                palettes = _model.Blocks.OfType<MapPaletteDataBlock>().Select(item => item.Value).ToList();

            SetPalettes(palettes);
        }
        internal void SetActionSet(IActionSetEntry propSet)
        {
            var propSetVM = new ActionSetVM();

            propSetVM.FromEntry(propSet);

            ActionSet = propSetVM;
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);

            //Layout.ToModel(_model.Layout);

            var mapEntry = entry as IMapEntry;

            mapEntry.ActionSetRef = ActionSet != null ? ActionSet.Id : null;
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

        //    Root.LevelEditor.BodyEditor.CurrentMapBody = Body;
        //}
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