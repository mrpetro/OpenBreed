using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenBreed.Model.Tiles
{
    public class TileModel
    {
        public TileModel(TileBuilder builder)
        {
            Index = builder.Index;
            Data = builder.Data;
        }
        public Rectangle Rectangle { get; internal set; }
        public int Index { get; private set; }
        public byte[] Data { get; private set; }
    }
}
