using OpenBreed.Common.Interface.Drawing;

namespace OpenBreed.Model.Tiles
{
    public class TileModel
    {
        public TileModel(TileBuilder builder)
        {
            Index = builder.Index;
            Data = builder.Data;
        }
        public MyRectangle Rectangle { get; internal set; }
        public int Index { get; private set; }
        public byte[] Data { get; private set; }
    }
}
