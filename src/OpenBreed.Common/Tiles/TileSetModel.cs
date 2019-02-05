﻿using OpenBreed.Common.Tiles.Builders;
using System.Collections.Generic;

namespace OpenBreed.Common.Tiles
{
    public enum TilePixelFormat
    {
        Indexed8bpp
    }

    public class TileSetModel
    {

        #region Public Constructors

        public TileSetModel(TileSetBuilder builder)
        {
            Tiles = builder.Tiles;
            TileSize = builder.TileSize;
            PixelFormat = builder.PixelFormat;
        }

        #endregion Public Constructors

        #region Public Properties

        public TilePixelFormat PixelFormat { get; private set; }

        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        public List<TileModel> Tiles { get; private set; }
        public int TileSize { get; private set; }

        #endregion Public Properties
    }
}