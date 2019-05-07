using OpenBreed.Common.Images.Builders;
using OpenBreed.Common.Images.Readers.ACBM;
using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Images.Readers.LBM;

namespace OpenBreed.Common.Formats
{
    public class LBMImageFormat : IDataFormatType
    {
        public LBMImageFormat()
        {
        }

        public object Load(AssetBase source, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            source.Stream.Seek(0, SeekOrigin.Begin);

            var imageBuilder = ImageBuilder.NewImage();
            var reader = new LBMImageReader(imageBuilder);
            return reader.Read(source.Stream);
        }

        public void Save(AssetBase source, object model, List<FormatParameter> parameters)
        {
            throw new NotImplementedException("LBMImageFormat Write");
        }
    }
}
