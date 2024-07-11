using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Model.Images;
using OpenBreed.Reader.Images;
using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace OpenBreed.Reader.Legacy.Images.ACBM
{
    public class ACBMImageReader : IImageReader
    {
        #region Private Fields

        private int _bitPlanesNo;

        private int _width;

        private int _height;

        private ACBMPaletteMode _paletteMode;

        #endregion Private Fields

        #region Public Constructors

        public ACBMImageReader(ImageBuilder builder, int width, int height, int bitPlanesNo, ACBMPaletteMode paletteMode)
        {
            Builder = builder;
            _width = width;
            _height = height;
            _paletteMode = paletteMode;

            _bitPlanesNo = bitPlanesNo;
        }

        #endregion Public Constructors

        #region Public Enums

        public enum ACBMPaletteMode
        {
            NONE,
            PALETTE_16BIT,
            PALETTE_32BIT
        }

        #endregion Public Enums

        #region Internal Properties

        internal ImageBuilder Builder { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public IImage Read(Stream stream)
        {
            Builder.SetSize(_width, _height);
            Builder.SetPixelFormat(MyPixelFormat.Format8bppIndexed);

            var binReader = new BigEndianBinaryReader(stream);
            binReader.BaseStream.Position = 0;
            int bytes = _width * _height / 8;

            byte[] imageData = new byte[_width * _height];

            for (int i = 0; i < _bitPlanesNo; i++)
            {
                byte[] rawData = binReader.ReadBytes(bytes);
                rawData = rawData.Reverse().ToArray();

                if (rawData.Length == 0)
                    return null;

                var bitArray = new BitArray(rawData);

                for (int k = 0; k < _width * _height; k++)
                    imageData[k] += (byte)((bitArray[k] ? 1 : 0) << i);
            }

            imageData = imageData.Reverse().ToArray();

            if (_paletteMode != ACBMPaletteMode.NONE)
            {
                var colorsNo = 0;
                MyColor[] colors;

                if (_bitPlanesNo < 6 || _bitPlanesNo > 6)
                {
                    colorsNo = (int)Math.Pow(2, _bitPlanesNo);
                    colors = new MyColor[colorsNo];

                    if (_paletteMode == ACBMPaletteMode.PALETTE_16BIT)
                    {
                        for (int i = 0; i < colorsNo; i++)
                            colors[i] = From16Bit(binReader.ReadUInt16());
                    }
                    else if (_paletteMode == ACBMPaletteMode.PALETTE_32BIT)
                    {
                        for (int i = 0; i < colorsNo; i++)
                            colors[i] = From32Bit(binReader.ReadUInt32());
                    }
                }
                else if (_bitPlanesNo == 6)
                {
                    colorsNo = 64; // Extra Half Byte (EHB)
                    colors = new MyColor[colorsNo];

                    for (int i = 0; i < 32; i++)
                        colors[i] = From16Bit(binReader.ReadUInt16());

                    for (int i = 32; i < 64; i++)
                        colors[i] = MyColor.FromArgb(
                            255,
                            (byte)(colors[i - 32].R / 2),
                            (byte)(colors[i - 32].G / 2),
                            (byte)(colors[i - 32].B / 2));
                }
                else if (_bitPlanesNo > 6)
                {
                    colorsNo = (int)Math.Pow(2, _bitPlanesNo);
                    colors = new MyColor[colorsNo];

                    for (int i = 0; i < colorsNo; i++)
                        colors[i] = From32Bit(binReader.ReadUInt32());
                }
                else
                    throw new NotImplementedException($"Not supported bit planes no: {_bitPlanesNo}");

                Builder.SetPalette(colors);
            }
            else
            {
                //Read default grayscale palette for better visibility of shapes
                var colors = new MyColor[256];

                for (int i = 0; i < 256; i++)
                {
                    if (i % 2 == 0)
                        colors[i] = MyColor.FromArgb(255, (byte)i, (byte)i, (byte)i);
                    else
                        colors[i] = MyColor.FromArgb(
                            (byte)255,
                            (byte)(255 - i),
                            (byte)(255 - i),
                            (byte)(255 - i));
                }

                Builder.SetPalette(colors);
            }

            //Start building new tile
            Builder.SetData(imageData);
            return Builder.Build();
        }

        #endregion Public Methods

        #region Internal Methods

        internal static MyColor From32Bit(UInt32 value)
        {
            byte b = (byte)(((value & 0x000000FF)));
            byte g = (byte)(((value & 0x0000FFFF) >> 8));
            byte r = (byte)(((value & 0x00FFFFFF) >> 16));
            return MyColor.FromArgb(255, r, g, b);
        }

        internal static MyColor From16Bit(UInt16 value)
        {
            byte r = (byte)(((value & 0x0FFF) >> 8) * 17);
            byte g = (byte)(((value & 0x00FF) >> 4) * 17);
            byte b = (byte)(((value & 0x000F)) * 17);
            return MyColor.FromArgb(255, r, g, b);
        }

        #endregion Internal Methods
    }
}