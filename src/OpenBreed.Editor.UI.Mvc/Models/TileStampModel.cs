using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Models
{
    public class TileStampModel : IModel
    {
        #region Private Fields

        private IDbTileStamp dbTileStamp;

        #endregion Private Fields

        #region Public Constructors

        public TileStampModel()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int CenterX => dbTileStamp.CenterX;
        public int CenterY => dbTileStamp.CenterY;

        public int Width => dbTileStamp.Width;
        public int Height => dbTileStamp.Height;

        public IReadOnlyList<IDbTileStampCell> Cells => dbTileStamp.Cells;

        #endregion Public Properties

        #region Public Methods

        public void Load(IDbTileStamp dbTileStamp)
        {
            this.dbTileStamp = dbTileStamp;
        }

        public void EraseTile(Vector4i erasePoint)
        {
            var borderBox = GetBorderBox();

            var tilePos = new MyPoint(erasePoint.X, erasePoint.Y);

            if (!borderBox.ContainsInclusive(new Vector2i(tilePos.X, tilePos.Y)))
            {
                return;
            }

            var foundCell = dbTileStamp.Cells.FirstOrDefault(item => item.X == tilePos.X && item.Y == tilePos.Y);

            if (foundCell is not null)
            {
                dbTileStamp.RemoveCell(foundCell);
            }
        }

        public void PutTiles(Vector4i insertPoint, string tileAtlasId, IReadOnlyList<TileSelection> toInsert)
        {
            if (toInsert.Count == 0)
            {
                return;
            }

            var borderBox = GetBorderBox();

            for (int i = 0; i < toInsert.Count; i++)
            {
                var tile = toInsert[i];
                var tilePos = new MyPoint(insertPoint.X + tile.Position.X, insertPoint.Y + tile.Position.Y);

                if (!borderBox.ContainsInclusive(new Vector2i(tilePos.X, tilePos.Y)))
                {
                    continue;
                }

                var foundCell = dbTileStamp.Cells.FirstOrDefault(item => item.X == tilePos.X && item.Y == tilePos.Y);

                if (foundCell is null)
                {
                    foundCell = dbTileStamp.AddNewCell(tilePos.X, tilePos.Y);
                }

                foundCell.TsId = tileAtlasId;
                foundCell.TsTi = tile.Index;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private Box2i GetBorderBox()
        {
            return new Box2i(0, 0, dbTileStamp.Width - 1, dbTileStamp.Height - 1);
        }

        #endregion Private Methods
    }
}