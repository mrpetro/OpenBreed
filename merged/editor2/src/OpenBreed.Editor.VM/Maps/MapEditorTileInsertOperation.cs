using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorTileInsertOperation
    {
        #region Public Constructors

        public MapEditorTileInsertOperation(Point indexCoords, int tileIdBefore, int tileIdAfter)
        {
            IndexCoords = indexCoords;
            TileIdBefore = tileIdBefore;
            TileIdAfter = tileIdAfter;
        }

        #endregion Public Constructors

        #region Public Properties

        public Point IndexCoords { get; }
        public int TileIdAfter { get; }
        public int TileIdBefore { get; }

        #endregion Public Properties
    }
}
