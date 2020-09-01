using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenBreed.Common.Commands;
using OpenBreed.Common.Model.Maps;

namespace OpenBreed.Editor.VM.Maps.Commands
{
    public class CmdTilesInsert : ICommand
    {
        #region Public Constructors

        public CmdTilesInsert(MapEditorTilesToolVM inserter, List<MapEditorTileInsertOperation> operations)
        {
            Inserter = inserter;
            Operations = operations;
        }

        #endregion Public Constructors

        #region Internal Properties

        internal MapEditorTilesToolVM Inserter { get; }
        internal List<MapEditorTileInsertOperation> Operations { get; }

        #endregion Internal Properties

        #region Public Methods

        public void Execute()
        {
            for (int i = 0; i < Operations.Count; i++)
            {
                Point tileCoords = Operations[i].IndexCoords;
                int tileId = Operations[i].TileIdAfter;

                Inserter.Layer.SetCell(tileCoords.X, tileCoords.Y, new TileRef(0, tileId));
            }

            //Inserter.Model.Update();
        }

        public void UnExecute()
        {
            for (int i = 0; i < Operations.Count; i++)
            {
                Point tileCoords = Operations[i].IndexCoords;
                int tileId = Operations[i].TileIdBefore;

                Inserter.Layer.SetCell(tileCoords.X, tileCoords.Y, new TileRef(0, tileId));
            }

            //Inserter.Model.Update();
        }

        #endregion Public Methods
    }
}
