using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Model.Sprites;
using OpenBreed.Reader.Legacy.Sprites.SPR;

namespace OpenBreed.Common.Formats
{
    public class ABTAODDSPRFormat : IDataFormatType
    {
        public ABTAODDSPRFormat()
        {
        }

        public object Load(DataSourceBase ds, List<FormatParameter> parameters)
        {

            var type = typeof((int, int));

            var n = type.Name;

            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var spriteSetBuilder = SpriteSetBuilder.NewSpriteSet();

            var spriteSizes = new List<(int, int)>();

            for (int i = 0; i < parameters.Count; i+=2)
            {
                var w = (int)parameters[i].Value;
                var h = (int)parameters[i + 1].Value;
                spriteSizes.Add((w, h));
            }

            var oddSprReader = new ODDSPRReader(spriteSetBuilder, spriteSizes.ToArray());
            return oddSprReader.Read(ds.Stream);
        }

        public void Save(DataSourceBase ds, object model, List<FormatParameter> parameters)
        {
            throw new NotImplementedException("ABTAODDSPR Write");
        }
    }
}
