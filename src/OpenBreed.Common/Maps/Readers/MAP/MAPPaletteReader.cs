using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Maps.Builders;
using OpenBreed.Common.Palettes.Builders;
using OpenBreed.Common.Readers;

namespace OpenBreed.Common.Maps.Readers.MAP
{
    public class MAPPaletteReader
    {
        private MAPReader _mainReader = null;

        public MAPPaletteReader(MAPReader mainReader)
        {
            _mainReader = mainReader;
        }

        public PaletteModel ReadEx(BigEndianBinaryReader binReader)
        {
            //TODO: For ABSE, it appears that palettes are missing in map files.

            UInt32 uIntSize = binReader.ReadUInt32();

            PaletteBuilder paletteBuilder = PaletteBuilder.NewPaletteModel();
            int colorsNo = (Int32)uIntSize / 8;
            paletteBuilder.CreateColors();

            for (int i = 0; i < colorsNo; i++)
            {
                byte[] colorData = binReader.ReadBytes(8);
                paletteBuilder.SetColor(i, Color.FromArgb(255, colorData[0], colorData[2], colorData[4]));
            }

            //NOTE: Temporary solution Read default grayscale palette for better visibility of shapes.
            for (int i = 0; i < 256; i++)
            {
                if (i % 2 == 0)
                    paletteBuilder.SetColor(i, Color.FromArgb(255, i, i, i));
                else
                    paletteBuilder.SetColor(i, Color.FromArgb(255, 255 - i, 255 - i, 255 - i));
            }

            return paletteBuilder.Build();
        }

        public PaletteModel Read(BigEndianBinaryReader binReader)
        {
            UInt32 uIntSize = binReader.ReadUInt32();

            PaletteBuilder paletteBuilder = PaletteBuilder.NewPaletteModel();
            int colorsNo = (Int32)uIntSize / 3;
            paletteBuilder.CreateColors();

            for (int i = 0; i < colorsNo; i++)
            {
                byte[] colorData = binReader.ReadBytes(3);
                paletteBuilder.SetColor(i,Color.FromArgb(255, colorData[0], colorData[1], colorData[2]));
            }

            for (int i = 64; i < colorsNo; i++)
            {
                paletteBuilder.SetColor(i, Color.FromArgb(255, i, i, i ));
            }

            return paletteBuilder.Build();
        }
    }
}
