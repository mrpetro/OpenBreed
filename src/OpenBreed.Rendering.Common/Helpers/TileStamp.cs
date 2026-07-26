using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Common.Builders;

namespace OpenBreed.Rendering.Common.Helpers
{
    internal class TileStampCell : ITileStampCell
    {
        public TileStampCell(int atlasId, int tileId)
        {
            AtlasId = atlasId;
            ImageId = tileId;
        }

        public int AtlasId { get; }
        public int ImageId { get; }
    }

    internal class TileStamp : ITileStamp
    {
        #region Internal Constructors

        internal TileStamp(StampBuilder builder)
        {
            Id = builder.GetId();
            Name = builder.name;
            Cells = builder.GetData();
            Width = builder.width;
            Height = builder.height;
            OriginX = builder.originX;
            OriginY = builder.originY;
            builder.Register(this);
        }

        #endregion Internal Constructors

        #region Public Properties

        public ITileStampCell[] Cells { get; }
        public int Id { get; }
        public string Name { get; }
        public int Width { get; }
        public int Height { get; }
        public int OriginX { get; }
        public int OriginY { get; }

        #endregion Public Properties
    }
}