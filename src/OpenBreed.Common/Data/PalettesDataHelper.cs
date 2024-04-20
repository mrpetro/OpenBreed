using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Model.Palettes;
using OpenBreed.Database.Interface.Items.Palettes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Model;
using OpenBreed.Reader.Legacy.Palettes;
using OpenBreed.Common.Interface.Data;
using System.Drawing.Imaging;
using OpenBreed.Common.Interface.Drawing;

namespace OpenBreed.Common.Data
{
    public class PalettesDataHelper
    {
        public static PaletteModel Create(MapPaletteBlock paletteBlock)
        {
            var paletteBuilder = PaletteBuilder.NewPaletteModel();
            paletteBuilder.SetName(paletteBlock.Name);
            paletteBuilder.CreateColors();
            for (int i = 0; i < paletteBlock.Value.Length; i++)
            {
                var colorData = paletteBlock.Value[i];
                paletteBuilder.SetColor(i, MyColor.FromArgb(255, colorData.R, colorData.G, colorData.B));
            }

            //for (int i = 64; i < paletteBlock.Value.Length; i++)
            //{
            //    var colorData = paletteBlock.Value[i];
            //    paletteBuilder.SetColor(i, Color.FromArgb(255, i, i, i));
            //}

            return paletteBuilder.Build();
        }

        public static PaletteModel Create(IColorPalette palette)
        {
            var paletteBuilder = PaletteBuilder.NewPaletteModel();
            paletteBuilder.SetName("Default");
            paletteBuilder.CreateColors();
            for (int i = 0; i < palette.Entries.Length; i++)
            {
                var colorData = palette.Entries[i];
                paletteBuilder.SetColor(i, MyColor.FromArgb(255, colorData.R, colorData.G, colorData.B));
            }

            return paletteBuilder.Build();
        }

        public static PaletteModel FromLbmImage(IModelsProvider dataProvider, IDbPaletteFromLbm entry)
        {
            var image = dataProvider.GetModel<IImage>(entry.DataRef);

            if (image is null)
                return null;

            return Create(image.Palette);
        }

        public static PaletteModel FromMapModel(IModelsProvider dataProvider, IDbPaletteFromMap entry)
        {
            var mapModel = dataProvider.GetModel<MapModel>(entry.DataRef);

            if (mapModel == null)
                return null;

            var paletteBlock = mapModel.Blocks.OfType<MapPaletteBlock>().FirstOrDefault(item => item.Name == entry.BlockName);

            if (paletteBlock == null)
                return null;

            return Create(paletteBlock);
        }

        public static PaletteModel FromBinary(IModelsProvider dataProvider, string dataRef, long dataStart, int colorsNo, PaletteMode paletteMode)
        {
            if (dataRef is null)
                return null;

            var binaryModel = dataProvider.GetModel<BinaryModel>(dataRef);

            if (binaryModel is null)
                return null;

            //Remember to set source stream to begining
            binaryModel.Stream.Seek(dataStart, SeekOrigin.Begin);

            var paletteBuilder = PaletteBuilder.NewPaletteModel();
            var paletteReader = new PaletteReader(paletteBuilder, ToPaletteMode(paletteMode), colorsNo);
            return paletteReader.Read(binaryModel.Stream);
        }

        public static PaletteModel FromBinary(IModelsProvider dataProvider, IDbPaletteFromBinary entry)
        {
            if (entry.DataRef == null)
                return null;

            var binaryModel = dataProvider.GetModel<BinaryModel>(entry.DataRef);

            if (binaryModel == null)
                return null;

            //Remember to set source stream to begining
            binaryModel.Stream.Seek(entry.DataStart, SeekOrigin.Begin);

            var paletteBuilder = PaletteBuilder.NewPaletteModel();
            var paletteReader = new PaletteReader(paletteBuilder, ToPaletteMode(entry.Mode), entry.ColorsNo);
            return paletteReader.Read(binaryModel.Stream);
        }

        public static PaletteReader.PaletteMode ToPaletteMode(PaletteMode mode)
        {
            switch (mode)
            {
                case PaletteMode.COLOR_16BIT:
                    return PaletteReader.PaletteMode.PALETTE_16BIT;
                case PaletteMode.COLOR_32BIT:
                    return PaletteReader.PaletteMode.PALETTE_32BIT;
                case PaletteMode.VGA_24BIT:
                    return PaletteReader.PaletteMode.VGA_24BIT;
                default:
                    throw new InvalidOperationException(mode.ToString());
            }
        }

    }
}
