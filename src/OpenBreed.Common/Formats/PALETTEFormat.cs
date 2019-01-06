using OpenBreed.Common.Palettes.Builders;
using OpenBreed.Common.Palettes.Readers;
using OpenBreed.Common.Assets;
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
        public object Load(AssetBase asset, List<FormatParameter> parameters)
        {
            var modeStr = (string)parameters.FirstOrDefault(item => item.Name == "MODE").Value;
            var colorsNo = (int)parameters.FirstOrDefault(item => item.Name == "COLORS_NO").Value;
            var dataStart = (int)parameters.FirstOrDefault(item => item.Name == "DATA_START").Value;

            var paletteMode = ToPaletteMode(modeStr);

            //Remember to set source stream to begining
            asset.Stream.Seek(dataStart, SeekOrigin.Begin);

            var paletteBuilder = PaletteBuilder.NewPaletteModel();
            var paletteReader = new PaletteReader(paletteBuilder, paletteMode, colorsNo);
            return paletteReader.Read(asset.Stream);
        }

        public void Save(AssetBase asset, object model)
        {
            throw new NotImplementedException();
        }
    }
}
