using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface
{
    /// <summary>
    /// Tile Grid interface for handling rendering of tile grids 
    /// </summary>
    public interface ITileGrid
    {
        #region Public Methods

        /// <summary>
        ///  Render tile grid using clipping box limits
        /// </summary>
        /// <param name="clipBox">Clipping box used to limit rendering</param>
        void Render(IRenderView view, Box2 clipBox);

        /// <summary>
        /// Modify single tile grid cell with new tile data using real world position
        /// </summary>
        /// <param name="pos">Position of tile grid cell to modify</param>
        /// <param name="tileAtlasId">ID of tile atlas which will be set on found cell</param>
        /// <param name="tileImageId">ID of tile image which will be set on found cell</param>
        void ModifyTile(Vector2 pos, int tileAtlasId, int tileImageId);

        /// <summary>
        /// Modify single tile grid cell with new tile data using tile grid index position
        /// </summary>
        /// <param name="pos">Index position of tile grid cell to modify</param>
        /// <param name="tileAtlasId">ID of tile atlas which will be set on found cell</param>
        /// <param name="tileImageId">ID of tile image which will be set on found cell</param>
        void ModifyTile(Vector2i pos, int tileAtlasId, int tileImageId);

        /// <summary>
        /// Modify multiple tile grid cells with tile stamp
        /// </summary>
        /// <param name="pos">Position of first tile grid cell to modify</param>
        /// <param name="stampId">ID of tile stamp which will be applied to cells</param>
        void ModifyTiles(Vector2 pos, int stampId);

        #endregion Public Methods
    }
}
