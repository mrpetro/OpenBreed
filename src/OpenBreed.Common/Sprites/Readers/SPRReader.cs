using OpenBreed.Common.Helpers;
using OpenBreed.Common.Model.Sprites;
using OpenBreed.Common.Model.Sprites.Builders;
using System;
using System.IO;

namespace OpenBreed.Common.Sprites.Readers.SPR
{
    public class SPRReader
    {
        #region Public Constructors

        public SPRReader(SpriteSetBuilder builder)
        {
            Builder = builder;
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

            Int16 spritesNo = binReader.ReadInt16();

            for (int i = 0; i < spritesNo; i++)
                ReadSprite(binReader, i);

            return Builder.Build();
        }

        #endregion Public Methods

        #region Private Methods

        private void ReadSprite(BinaryReader binReader, int index)
        {
            //Start building new sprite
            var spriteBuilder = SpriteBuilder.NewSprite();

            spriteBuilder.SetIndex(index);

            //Read sprite header data(width, height and sprite bitmap data offset)
            int width = binReader.ReadInt16();
            width = MathHelper.ToNextPowOf2(width);
            int height = binReader.ReadInt16();
            height = MathHelper.ToNextPowOf2(height);
            spriteBuilder.SetSize(width, height);
            UInt16 offset = binReader.ReadUInt16();

            //Remember sprites headers data position
            long currentHeadPos = binReader.BaseStream.Position;
            //Jump with reader to sprite data
            binReader.BaseStream.Seek(offset, SeekOrigin.Begin);
            //Read the sprite bitmap data
            ReadSpriteBitmap(binReader, spriteBuilder);
            //Jump back to sprite headers data
            binReader.BaseStream.Seek(currentHeadPos, SeekOrigin.Begin);

            //Add new sprite to sprite set being build
            Builder.AddSprite(spriteBuilder.Build());
        }

        private void ReadSpriteBitmap(BinaryReader binReader, SpriteBuilder spriteBuilder)
        {
            byte[] spriteData = new byte[spriteBuilder.Width * spriteBuilder.Height];

            while (true)
            {
                byte lineNo = binReader.ReadByte();

                if (lineNo == 0xFF)
                    break;

                byte lineStart = binReader.ReadByte();
                byte lineLength = binReader.ReadByte();
                var lineBytes = binReader.ReadBytes(lineLength);

                for (byte i = 0; i < lineLength; i++)
                    spriteData[(lineStart + i) * spriteBuilder.Width + lineNo] = lineBytes[i];
            }

            spriteBuilder.SetData(spriteData);
        }

        #endregion Private Methods
    }
}