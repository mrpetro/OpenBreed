using OpenBreed.Common.XmlDatabase.Items.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables
{
    public class XmlDbImageTableDef : XmlDbTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("Image", typeof(XmlImageEntry))]
        public readonly List<XmlImageEntry> Items = new List<XmlImageEntry>();
    }
}
