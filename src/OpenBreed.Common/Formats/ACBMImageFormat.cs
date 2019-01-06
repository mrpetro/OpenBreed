using OpenBreed.Common.Images.Builders;
using OpenBreed.Common.Images.Readers.ACBM;
using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class ACBMImageFormat : IDataFormatType
    {
        public ACBMImageFormat()
        {
        }

        public static ACBMImageReader.ACBMPaletteMode ToACBMPaletteMode(string paletteMode)
        {
            switch (paletteMode)
            {
                case "NONE":
                    return ACBMImageReader.ACBMPaletteMode.NONE;
                case "16BIT":
                    return ACBMImageReader.ACBMPaletteMode.PALETTE_16BIT;
                case "32BIT":
                    return ACBMImageReader.ACBMPaletteMode.PALETTE_32BIT;
                default:
                    throw new InvalidOperationException(paletteMode);
            }
        }

        public object Load(AssetBase source, List<FormatParameter> parameters)
        {
            var width = (int)parameters.FirstOrDefault(item=> item.Name == "WIDTH").Value;
            var height = (int)parameters.FirstOrDefault(item => item.Name == "HEIGHT").Value;
            var bitPlanesNo = (int)parameters.FirstOrDefault(item => item.Name == "BIT_PLANES_NO").Value;
            var paletteStr = (string)parameters.FirstOrDefault(item => item.Name == "PALETTE_MODE").Value;

            var paletteMode = ToACBMPaletteMode(paletteStr);

            //Remember to set source stream to begining
            source.Stream.Seek(0, SeekOrigin.Begin);

            var imageBuilder = ImageBuilder.NewImage();
            var reader = new ACBMImageReader(imageBuilder, width, height, bitPlanesNo, paletteMode);
            return reader.Read(source.Stream);
        }

        public void Save(AssetBase source, object model)
        {
            throw new NotImplementedException("ACBMImage Write");
        }
    }
}
