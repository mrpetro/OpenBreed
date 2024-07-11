using OpenBreed.Database.Xml.Items.Images;
using OpenBreed.Database.Xml.Items.Palettes;
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
        XmlArrayItem("IffImage", typeof(XmlDbIffImage)),
        XmlArrayItem("AcbmImage", typeof(XmlDbAcbmImage))]
        public readonly List<XmlDbImage> Items = new List<XmlDbImage>();
    }
}
