using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Model.Actions;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapVM : EditableEntryVM
    {
        #region Private Fields

        private MapModel _model;
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

            Palettes = new BindingList<PaletteModel>();
            Palettes.ListChanged += (s, e) => OnPropertyChanged(nameof(Palettes));

            PropertyChanged += MapVM_PropertyChanged;

            Layout.PropertyChanged += (s, e) => OnPropertyChanged(nameof(Layout));
        }

        #endregion Public Constructors

        #region Public Properties

        public bool IsModified
        {
            get { return _isModified; }
            internal set { SetProperty(ref _isModified, value); }
        }

        public MapLayoutVM Layout { get; }

        public BindingList<PaletteModel> Palettes { get; }
        public LevelPropertiesVM Properties { get; }

        public AssetBase Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }

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

        public void SetPalettes(List<PaletteModel> palettes)
        {
            Palettes.UpdateAfter(() =>
            {
                Palettes.Clear();
                palettes.ForEach(palette => Palettes.Add(palette));
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

        #endregion Public Methods

        #region Internal Methods

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);

            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var mapEntry = entry as IMapEntry;

            if (mapEntry.TileSetRef != null)
            {
                var tileSet = dataProvider.TileSets.GetTileSet(mapEntry.TileSetRef);
                SetTileSets(new List<TileSetModel>(new TileSetModel[] { tileSet }));
            }

            foreach (var spriteSetRef in mapEntry.SpriteSetRefs)
            {
                var spriteSet = dataProvider.SpriteSets.GetSpriteSet(spriteSetRef);
                //AddSpriteSet(spriteSet);
            }

            _model = dataProvider.Maps.GetMap(entry.Id);

            Properties.Load(_model);

            Layout.FromMap(_model);

            var palettes = new List<PaletteModel>();

            foreach (var paletteRef in mapEntry.PaletteRefs)
                palettes.Add(dataProvider.Palettes.GetPalette(paletteRef));

            SetPalettes(palettes);
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);

            Layout.ToMap(_model);
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
                return Source.Id + "*";
            else
                return Source.Id;
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