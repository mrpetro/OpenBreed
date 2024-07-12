using OpenBreed.Database.Xml.Items.Songs;
using OpenBreed.Database.Xml.Items.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbSongTableDef : XmlDbTableDef
    {
        public const string NAME = "Songs";

        [XmlIgnore]
        public override string Name => NAME;

        [XmlArray("Items"),
        XmlArrayItem("Song", typeof(XmlDbSong))]
        public readonly List<XmlDbSong> Items = new List<XmlDbSong>();
    }
}
