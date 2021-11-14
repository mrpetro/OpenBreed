using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenBreed.Common.Commands;
using OpenBreed.Model.Maps;

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
            //Inserter.StartModelUpdate();

            for (int i = 0; i < Operations.Count; i++)
            {
                Point tileCoords = Operations[i].IndexCoords;
                int tileId = Operations[i].TileIdAfter;

                Inserter.SetValue(tileCoords, tileId);
            }

            //Inserter.Model.Update();

            //Inserter.FinishModelUpdate();
        }

        public void UnExecute()
        {
            for (int i = 0; i < Operations.Count; i++)
            {
                Point tileCoords = Operations[i].IndexCoords;
                int tileId = Operations[i].TileIdBefore;

                Inserter.Layout.SetCellValue(Inserter.LayerIndex, tileCoords.X, tileCoords.Y, tileId);
            }

            //Inserter.Model.Update();
        }

        #endregion Public Methods
    }
}
