using OpenBreed.Model.Sprites;
using OpenBreed.Reader.Sprites;
using System;
using System.IO;

namespace OpenBreed.Reader.Legacy.Sprites.SPR
{
    public class ODDSPRReader : ISpriteSetReader
    {
        private readonly (int, int)[] spriteSizes;

        #region Public Constructors

        public ODDSPRReader(SpriteSetBuilder builder, (int, int)[] spriteSizes = null)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.spriteSizes = spriteSizes ?? throw new ArgumentNullException(nameof(spriteSizes));
        }

        #endregion Public Constructors

        #region Internal Properties

        internal SpriteSetBuilder Builder { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public SpriteSetModel Read(Stream stream)
        {
            //We dont want to close the stream here so reader is not being used inside of 'using' statement
            //BigEndianBinaryReader binReader = new BigEndianBinaryReader(stream);
            BinaryReader binReader = new BinaryReader(stream);

            for (int i = 0; i < spriteSizes.Length; i++)
            {
                //Start building new sprite
                var spriteBuilder = SpriteBuilder.NewSprite();
                spriteBuilder.SetIndex(i);

                var size = spriteSizes[i];
                ReadSpriteBitmap(binReader, spriteBuilder, size.Item1, size.Item2);

                //Add new sprite to sprite set being build
                Builder.AddSprite(spriteBuilder.Build());
            }

            return Builder.Build();
        }

        #endregion Public Methods

        #region Private Methods

        private void ReadSpriteBitmap(BinaryReader binReader, SpriteBuilder spriteBuilder, int width, int height)
        {
            spriteBuilder.SetSize(width, height);

            var spriteData = new byte[width * height];

            var lineBytes = binReader.ReadBytes(width * height);

            var width4 = width / 4;
            var size = width4 * height;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width4; i++)
                {
                    spriteData[0 + j * width + i * 4] = lineBytes[0 * size + i + j * width4];
                    spriteData[1 + j * width + i * 4] = lineBytes[1 * size + i + j * width4];
                    spriteData[2 + j * width + i * 4] = lineBytes[2 * size + i + j * width4];
                    spriteData[3 + j * width + i * 4] = lineBytes[3 * size + i + j * width4];
                }
            }

            spriteBuilder.SetData(spriteData);
        }

        #endregion Private Methods
    }
}