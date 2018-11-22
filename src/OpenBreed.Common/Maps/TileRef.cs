using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Maps
{
    public struct TileRef
    {
        #region Public Constructors

        public TileRef(int tileSetId, int tileId)
        {
            TileSetId = tileSetId;
            TileId = tileId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TileId { get; private set; }
        public int TileSetId { get; private set; }

        #endregion Public Properties
    }
}
