using OpenBreed.Common.Palettes.Builders;
using OpenBreed.Common.Palettes.Readers;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class PaletteFormat : IDataFormatType
    {
        public PaletteFormat()
        {
        }

        public static PaletteReader.PaletteMode ToPaletteMode(string paletteModeStr)
        {
            switch (paletteModeStr)
            {
                case "16BIT":
                    return PaletteReader.PaletteMode.PALETTE_16BIT;
                case "32BIT":
                    return PaletteReader.PaletteMode.PALETTE_32BIT;
                default:
                    throw new InvalidOperationException(paletteModeStr);
            }
        }
        public object Load(SourceBase source, Dictionary<string, object> parameters = null)
        {
            var modeStr = (string)parameters["MODE"];
            var colorsNo = (int)parameters["COLORS_NO"];
            var dataStart = (int)parameters["DATA_START"];

            var paletteMode = ToPaletteMode(modeStr);

            //Remember to set source stream to begining
            source.Stream.Seek(dataStart, SeekOrigin.Begin);

            var paletteBuilder = PaletteBuilder.NewPaletteModel();
            var paletteReader = new PaletteReader(paletteBuilder, paletteMode, colorsNo);
            return paletteReader.Read(source.Stream);
        }

        public void Save(SourceBase source, object model)
        {
            throw new NotImplementedException();
        }
    }
}
