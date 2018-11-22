using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Props.Builders;
using OpenBreed.Common.Props.Readers.XML;

namespace OpenBreed.Editor.VM.Sources.Formats
{
    public class PropertySetXMLFormat : ISourceFormat
    {
        public PropertySetXMLFormat()
        {
        }

        public object Load(BaseSource source)
        {
            //Remember to set source stream to begining
            source.Stream.Seek(0, SeekOrigin.Begin);

            var propertySetBuilder = PropertySetBuilder.NewPropertySet();
            PropertySetDefReader propertySetDefReader = new PropertySetDefReader(propertySetBuilder);
            return propertySetDefReader.Read(source.Stream);
        }

        public void Save(BaseSource source, object model)
        {
            throw new NotImplementedException("PropertySetXML Write");
        }
    }
}
