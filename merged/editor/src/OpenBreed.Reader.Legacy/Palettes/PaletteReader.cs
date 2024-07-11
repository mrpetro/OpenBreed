using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Model.Palettes;
using OpenBreed.Reader.Palettes;
using System;
using System.IO;

namespace OpenBreed.Reader.Legacy.Palettes
{
    public class PaletteReader : IPaletteReader
    {
        #region Public Constructors

        public PaletteReader(PaletteBuilder builder, PaletteMode mode, int colorsNo)
        {
            Builder = builder;
            Mode = mode;
            ColorsNo = colorsNo;
        }

        #endregion Public Constructors

        #region Public Enums

        public enum PaletteMode
        {
            PALETTE_16BIT,
            PALETTE_32BIT,
            VGA_24BIT
        }

        #endregion Public Enums

        #region Internal Properties

        internal PaletteBuilder Builder { get; }
        internal int ColorsNo { get; }
        internal PaletteMode Mode { get; }

        #endregion Internal Properties

        #region Public Methods

        public PaletteModel Read(Stream stream)
        {
            if (Mode == PaletteMode.PALETTE_16BIT)
                return Read16BitPalette(stream);
            else if (Mode == PaletteMode.PALETTE_32BIT)
                return Read32BitPalette(stream);
            else if (Mode == PaletteMode.VGA_24BIT)
                return ReadVga24BitPalette(stream);
            else throw new InvalidOperationException(Mode.ToString());
        }

        #endregion Public Methods

        #region Internal Methods

        internal static MyColor From16Bit(UInt16 value)
        {
            byte r = (byte)(((value & 0x0FFF) >> 8) * 17);
            byte g = (byte)(((value & 0x00FF) >> 4) * 17);
            byte b = (byte)(((value & 0x000F)) * 17);
            return MyColor.FromArgb(255, r, g, b);
        }

        internal static MyColor From32Bit(UInt32 value)
        {
            byte b = (byte)(((value & 0x000000FF)));
            byte g = (byte)(((value & 0x0000FFFF) >> 8));
            byte r = (byte)(((value & 0x00FFFFFF) >> 16));
            return MyColor.FromArgb(255, r, g, b);
        }

        internal static MyColor FromVga24BitColor(byte[] value)
        {
            var r = (byte)(((int)value[0]) << 2);
            var g = (byte)(((int)value[1]) << 2);
            var b = (byte)(((int)value[2]) << 2);
            return MyColor.FromArgb(255, r, g, b);
        }

        #endregion Internal Methods

        #region Private Methods

        private PaletteModel Read16BitPalette(Stream stream)
        {
            //We dont want to close the stream here so reader is not being used inside of 'using' statement
            var binReader = new BigEndianBinaryReader(stream);

            Builder.CreateColors();

            for (int i = 0; i < ColorsNo; i++)
                Builder.SetColor(i, From16Bit(binReader.ReadUInt16()));

            return Builder.Build();
        }

        private PaletteModel Read32BitPalette(Stream stream)
        {
            //We dont want to close the stream here so reader is not being used inside of 'using' statement
            var binReader = new BigEndianBinaryReader(stream);

            for (int i = 0; i < ColorsNo; i++)
                Builder.SetColor(i, From32Bit(binReader.ReadUInt32()));

            return Builder.Build();
        }

        private PaletteModel ReadVga24BitPalette(Stream stream)
        {
            //We dont want to close the stream here so reader is not being used inside of 'using' statement
            var binReader = new BigEndianBinaryReader(stream);

            for (int i = 0; i < ColorsNo; i++)
                Builder.SetColor(i, FromVga24BitColor(binReader.ReadBytes(3)));

            return Builder.Build();
        }

        #endregion Private Methods
    }
}