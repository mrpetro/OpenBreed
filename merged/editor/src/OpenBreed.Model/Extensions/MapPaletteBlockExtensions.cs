using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Model.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Model.Extensions
{
    public static class MapPaletteBlockExtensions
    {
        public static PaletteModel ToPaletteModel(this MapPaletteBlock paletteBlock)
        {
            var paletteBuilder = PaletteBuilder.NewPaletteModel();
            paletteBuilder.SetName(paletteBlock.Name);
            paletteBuilder.CreateColors();
            for (int i = 0; i < paletteBlock.Value.Length; i++)
            {
                var colorData = paletteBlock.Value[i];
                paletteBuilder.SetColor(i, MyColor.FromArgb(255, colorData.R, colorData.G, colorData.B));
            }

            return paletteBuilder.Build();
        }
    }
}
