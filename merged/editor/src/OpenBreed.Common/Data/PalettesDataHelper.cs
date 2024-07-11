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
using OpenBreed.Model.Extensions;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Images;

namespace OpenBreed.Common.Data
{
    public class PalettesDataHelper
    {
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
            var image = dataProvider.GetModelById<IDbImage, IImage>(entry.ImageRef);

            if (image is null)
                return null;

            return Create(image.Palette);
        }

        public static PaletteModel FromMapModel(MapModel mapModel, IDbPaletteFromMap entry)
        {
            var paletteBlock = mapModel.Blocks.OfType<MapPaletteBlock>().FirstOrDefault(item => item.Name == entry.BlockName);

            if (paletteBlock is null)
            {
                return null;
            }

            return paletteBlock.ToPaletteModel();
        }

        public static PaletteModel FromMapModel(IModelsProvider dataProvider, IDbPaletteFromMap entry)
        {
            var mapModel = dataProvider.GetModelById<IDbMap, MapModel>(entry.MapRef);

            if (mapModel is null)
            {
                return null;
            }

            return FromMapModel(mapModel, entry);
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
