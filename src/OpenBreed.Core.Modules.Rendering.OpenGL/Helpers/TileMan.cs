using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public class TileMan : ITileMan
    {
        #region Private Fields

        private readonly List<ITileAtlas> items = new List<ITileAtlas>();

        #endregion Private Fields

        #region Public Methods

        public ITileAtlas Create(ITexture texture, int tileSize, int tileColumns, int tileRows)
        {
            var newTileAtlas = new TileAtlas(items.Count, texture, tileSize, tileColumns, tileRows);
            items.Add(newTileAtlas);
            return newTileAtlas;
        }

        public ITileAtlas GetById(int id)
        {
            return items[id];
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}