using OpenBreed.Database.Xml.Items.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbTextTableDef : XmlDbTableDef
    {
        public const string NAME = "Texts";

        [XmlIgnore]
        public override string Name => NAME;

        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("TextEmbedded", typeof(XmlDbTextEmbedded)),
        XmlArrayItem("TextFromMAP", typeof(XmlDbTextFromMap)),
        XmlArrayItem("TextFromFile", typeof(XmlDbTextFromFile)),]
        public readonly List<XmlDbText> Items = new List<XmlDbText>();

        #endregion Public Fields
    }
}
