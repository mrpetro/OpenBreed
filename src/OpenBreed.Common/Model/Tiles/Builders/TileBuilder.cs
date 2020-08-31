using OpenBreed.Common.Model.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Model.Tiles.Builders
{
    public class TileBuilder
    {
        internal int Index;
        internal byte[] Data = null;

        public TileBuilder SetIndex(int index)
        {
            Index = index;
            return this;
        }

        public TileBuilder SetData(byte[] data)
        {
            Data = data;
            return this;
        }

        public static TileBuilder NewTile()
        {
            return new TileBuilder();
        }

        public TileModel Build()
        {
            return new TileModel(this);
        }
    }
}
