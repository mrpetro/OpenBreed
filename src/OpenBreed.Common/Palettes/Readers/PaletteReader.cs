using OpenBreed.Common.Palettes.Builders;
using OpenBreed.Common.Readers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Palettes.Readers
{
    public class PaletteReader
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
            PALETTE_32BIT
        }

        #endregion Public Enums

        #region Internal Properties

        internal PaletteBuilder Builder { get; }
        internal int ColorsNo { get; }
        internal PaletteMode Mode { get; }

        #endregion Internal Properties

        #region Public Methods

        public static Color From16Bit(UInt16 value)
        {
            byte r = (byte)(((value & 0x0FFF) >> 8) * 17);
            byte g = (byte)(((value & 0x00FF) >> 4) * 17);
            byte b = (byte)(((value & 0x000F)) * 17);
            return Color.FromArgb(255, r, g, b);
        }

        public static Color From32Bit(UInt32 value)
        {
            byte b = (byte)(((value & 0x000000FF)));
            byte g = (byte)(((value & 0x0000FFFF) >> 8));
            byte r = (byte)(((value & 0x00FFFFFF) >> 16));
            return Color.FromArgb(255, r, g, b);
        }

        public PaletteModel Read(Stream stream)
        {
            if (Mode == PaletteMode.PALETTE_16BIT)
                return Read16Bit(stream);
            else if (Mode == PaletteMode.PALETTE_16BIT)
                return Read32Bit(stream);
            else throw new InvalidOperationException(Mode.ToString());
        }

        #endregion Public Methods

        #region Private Methods

        private PaletteModel Read16Bit(Stream stream)
        {
            //We dont want to close the stream here so reader is not being used inside of 'using' statement
            BigEndianBinaryReader binReader = new BigEndianBinaryReader(stream);

            Builder.CreateColors();

            for (int i = 0; i < ColorsNo; i++)
                Builder.SetColor(i, From16Bit(binReader.ReadUInt16()));

            return Builder.Build();
        }

        private PaletteModel Read32Bit(Stream stream)
        {
            //We dont want to close the stream here so reader is not being used inside of 'using' statement
            BigEndianBinaryReader binReader = new BigEndianBinaryReader(stream);

            for (int i = 0; i < ColorsNo; i++)
                Builder.SetColor(i, From32Bit(binReader.ReadUInt32()));

            return Builder.Build();
        }

        #endregion Private Methods
    }
}
