using OpenBreed.Common.Maps;
using OpenBreed.Common.Maps.Builders;
using OpenBreed.Common.Maps.Readers.MAP;
using OpenBreed.Common.Maps.Writers.MAP;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class ABSEMAPFormat : IDataFormat
    {
        public ABSEMAPFormat()
        {
        }

        public object Load(SourceBase source, Dictionary<string, object> parameters = null)
        {
            //Remember to set source stream to begining
            source.Stream.Seek(0, SeekOrigin.Begin);

            var mapBuilder = MapBuilder.NewMapModel();
            MAPReader mapReader = new MAPReader(mapBuilder, MAPFormat.ABSE);
            return mapReader.Read(source.Stream);
        }

        public void Save(SourceBase source, object model)
        {
            if (source.Stream == null)
                throw new InvalidOperationException("Source stream not opened.");

            //Remember to clear the stream before writing
            source.Stream.SetLength(0);

            MAPWriter mapWriter = new MAPWriter(source.Stream);
            mapWriter.Write((MapModel)model);
        }
    }
}
