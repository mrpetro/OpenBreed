using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Builders;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    public class TileStamp : ITileStamp
    {
        #region Internal Constructors

        internal TileStamp(StampBuilder builder)
        {
            Id = builder.GetId();
            Data = builder.GetData();
            Width = builder.width;
            Height = builder.height;
            OriginX = builder.originX;
            OriginY = builder.originY;
            builder.Register(this);
        }

        #endregion Internal Constructors

        #region Public Properties

        public int[] Data { get; }
        public int Id { get; }
        public int Width { get; }
        public int Height { get; }
        public int OriginX { get; }
        public int OriginY { get; }

        #endregion Public Properties
    }
}