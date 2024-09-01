using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface.Items.TileStamps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface.Data
{
    public interface ITileStampDataLoader : IDataLoader<ITileStamp>
    {
        #region Public Methods

        ITileStamp Load(IDbTileStamp dbTileStamp);

        #endregion Public Methods
    }
}