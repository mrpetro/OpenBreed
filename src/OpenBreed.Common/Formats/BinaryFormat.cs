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
    public class BinaryFormat : IDataFormatType
    {
        public BinaryFormat()
        {
        }

        public object Load(AssetBase asset, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            asset.Stream.Seek(0, SeekOrigin.Begin);

            using (var br = new BinaryReader(asset.Stream,Encoding.Default, true))
                return new BinaryModel(br.ReadBytes((int)asset.Stream.Length));
        }

        public void Save(AssetBase asset, object model, List<FormatParameter> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
