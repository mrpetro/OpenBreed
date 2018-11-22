using OpenBreed.Common.Sprites.Builders;

namespace OpenBreed.Common.Sprites
{
    public class SpriteModel
    {
        #region Public Constructors

        public SpriteModel(SpriteBuilder builder)
        {
            Index = builder.Index;
            Width = builder.Width;
            Height = builder.Height;
            Data = builder.Data;
        }

        #endregion Public Constructors

        #region Public Properties

        public byte[] Data { get; private set; }
        public int Height { get; private set; }
        public int Index { get; private set; }
        public int Width { get; private set; }

        #endregion Public Properties
    }
}