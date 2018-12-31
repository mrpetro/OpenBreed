using OpenBreed.Common.XmlDatabase.Items.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables.Images
{
    public class DatabaseImageTableDef : DatabaseTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("Image", typeof(ImageDef))]
        public readonly List<ImageDef> Items = new List<ImageDef>();
    }
}
