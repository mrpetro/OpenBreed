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
 
            TileSetSelector.Connect();
            TilesSelector.Connect();
        }

        #endregion Public Methods

    }
}
