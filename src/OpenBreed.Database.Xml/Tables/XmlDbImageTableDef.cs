using OpenBreed.Database.Xml.Items.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbImageTableDef : XmlDbTableDef
    {
        public const string NAME = "Images";

        [XmlIgnore]
        public override string Name => NAME;

        [XmlArray("Items"),
        XmlArrayItem("Image", typeof(XmlImageEntry))]
        public readonly List<XmlImageEntry> Items = new List<XmlImageEntry>();
    }
}
