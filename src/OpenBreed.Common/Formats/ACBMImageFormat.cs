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

        public object Load(BaseSource source, Dictionary<string, object> parameters = null)
        {
            var width = (int)parameters["WIDTH"];
            var height = (int)parameters["HEIGHT"];
            var bitPlanesNo = (int)parameters["BIT_PLANES_NO"];
            var paletteStr = (string)parameters["PALETTE_MODE"];

            ACBMImageReader.ACBMPaletteMode paletteMode;

            if (paletteStr == "NONE")
                paletteMode = ACBMImageReader.ACBMPaletteMode.NONE;
            else if (paletteStr == "16BIT")
                paletteMode = ACBMImageReader.ACBMPaletteMode.PALETTE_16BIT;
            else if (paletteStr == "32BIT")
                paletteMode = ACBMImageReader.ACBMPaletteMode.PALETTE_32BIT;
            else
                throw new InvalidOperationException(paletteStr);

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
