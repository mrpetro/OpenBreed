using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Editor.VM.Levels.Readers.XML;
using OpenBreed.Editor.VM.Levels.Builders;
using OpenBreed.Common;

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

        //public object Load(BaseSource source)
        //{
        //    //Remember to set source stream to begining
        //    source.Stream.Seek(0, SeekOrigin.Begin);

        //    var levelBuilder = LevelBuilder.NewLevel();
        //    levelBuilder.SetSource(source);
        //    LevelDefReader levelDefReader = new LevelDefReader(levelBuilder);
        //    return levelDefReader.Read(source.Stream);
        //}

        public void Save(BaseSource source, object model)
        {
            throw new NotImplementedException("LevelXML Write");
        }
    }
}
