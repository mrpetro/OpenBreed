using OpenBreed.Common.Images.Builders;
using OpenBreed.Common.Images.Readers.ACBM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Sources.Formats
{
    public class ACBMImageFormat : ISourceFormat
    {
        public ACBMImageFormat()
        {
        }

        public object Load(BaseSource source)
        {
            int width = source.GetParameter<int>("WIDTH");
            int height = source.GetParameter<int>("HEIGHT");
            int bitPlanesNo = source.GetParameter<int>("BIT_PLANES_NO");
            var paletteStr = source.GetParameter<string>("PALETTE_MODE");

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
