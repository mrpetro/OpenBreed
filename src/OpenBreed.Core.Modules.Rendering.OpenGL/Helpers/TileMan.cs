using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    internal class TileMan : ITileMan
    {
        #region Private Fields

        private readonly List<ITileAtlas> items = new List<ITileAtlas>();

        #endregion Private Fields

        #region Internal Constructors

        internal TileMan(OpenGLModule module)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal OpenGLModule Module { get; }

        #endregion Internal Properties

        #region Public Methods

        public ITileAtlas Create(int textureId, int tileSize, int tileColumns, int tileRows)
        {
            var texture = Module.Textures.GetById(textureId);
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