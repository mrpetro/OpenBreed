using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Sprites.Builders;
using OpenBreed.Common.Sprites.Readers.SPR;
using OpenBreed.Common.Assets;

namespace OpenBreed.Common.Formats
{
    public class ABTASPRFormat : IDataFormatType
    {
        public ABTASPRFormat()
        {
        }

        public object Load(AssetBase asset, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            asset.Stream.Seek(0, SeekOrigin.Begin);

            var spriteSetBuilder = SpriteSetBuilder.NewSpriteSet();
            //spriteSetBuilder.SetSource(source);
            SPRReader sprReader = new SPRReader(spriteSetBuilder);
            return sprReader.Read(asset.Stream);
        }

        public void Save(AssetBase source, object model, List<FormatParameter> parameters)
        {
            throw new NotImplementedException("ABTASPR Write");
        }
    }
}
