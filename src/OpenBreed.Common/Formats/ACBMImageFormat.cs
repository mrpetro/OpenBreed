using OpenBreed.Common.Images.Builders;
using OpenBreed.Common.Images.Readers.ACBM;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class ACBMImageFormat : IDataFormat
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

        public object Load(BaseSource source, Dictionary<string, object> parameters = null)
        {
            var width = (int)parameters["WIDTH"];
            var height = (int)parameters["HEIGHT"];
            var bitPlanesNo = (int)parameters["BIT_PLANES_NO"];
            var paletteStr = (string)parameters["PALETTE_MODE"];

            var paletteMode = ToACBMPaletteMode(paletteStr);

            //Remember to set source stream to begining
            source.Stream.Seek(0, SeekOrigin.Begin);

            var imageBuilder = ImageBuilder.NewImage();
            var reader = new ACBMImageReader(imageBuilder, width, height, bitPlanesNo, paletteMode);
            return reader.Read(source.Stream);
        }

        public void Save(BaseSource source, object model)
        {
            throw new NotImplementedException("ACBMImage Write");
        }
    }
}
