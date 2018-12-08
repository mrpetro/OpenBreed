using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common;
using OpenBreed.Common.Database.Items.Levels;

namespace OpenBreed.Editor.VM.Sources.Formats
{
    public class LevelXMLFormat : ISourceFormat
    {
        public LevelXMLFormat()
        {
        }

        public object Load(BaseSource source)
        {
            //Remember to set source stream to begining
            source.Stream.Seek(0, SeekOrigin.Begin);

            return Tools.RestoreFromXml<LevelDef>(source.Stream);
        }

        public void Save(BaseSource source, object model)
        {
            throw new NotImplementedException("LevelXML Write");
        }
    }
}
