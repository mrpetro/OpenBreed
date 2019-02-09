using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorTilesToolVM : BaseViewModel
    {

        #region Public Constructors

        public MapEditorTilesToolVM(MapEditorVM parent)
        {
            Parent = parent;

            TileSetSelector = new MapEditorTileSetSelectorVM(this);
            TilesSelector = new MapEditorTilesSelectorVM(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public MapEditorVM Parent { get; }
        public MapEditorTileSetSelectorVM TileSetSelector { get; }
        public MapEditorTilesSelectorVM TilesSelector { get; }

        #endregion Public Properties

        #region Public Methods

        public void Connect()
        {
            Parent.PropertyChanged += MapEditor_PropertyChanged;
            TileSetSelector.PropertyChanged += TileSetSelector_PropertyChanged;
        }

        #endregion Public Methods

        #region Private Methods

        private void MapEditor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var mapEditor = sender as MapEditorVM;

            switch (e.PropertyName)
            {
                case nameof(mapEditor.Editable):
                    OnCurrentMapChanged(mapEditor.Editable);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This method will update this tool tile sets with map tile sets
        /// </summary>
        /// <param name="map">Map that has changed</param>
        private void OnCurrentMapChanged(MapVM map)
        {
            TileSetSelector.TileSets.UpdateAfter(() =>
            {
                TileSetSelector.TileSets.Clear();
                if (map != null)
                    map.TileSets.ForEach(item => TileSetSelector.TileSets.Add(item));
            });

            TileSetSelector.CurrentItem = TileSetSelector.TileSets.FirstOrDefault();
        }

        private void OnCurrentTileSetChanged(TileSetVM tileSet)
        {
            TilesSelector.CurrentTileSet = tileSet;
        }

        private void TileSetSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var tileSetSelector = sender as MapEditorTileSetSelectorVM;

            switch (e.PropertyName)
            {
                case nameof(tileSetSelector.CurrentItem):
                    OnCurrentTileSetChanged(tileSetSelector.CurrentItem);
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}
