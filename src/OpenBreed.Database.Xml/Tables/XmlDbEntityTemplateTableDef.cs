using OpenBreed.Database.Xml.Items.EntityTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbEntityTemplateTableDef : XmlDbTableDef
    {
        public const string NAME = "EntityTemplates";

        [XmlIgnore]
        public override string Name => NAME;

        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("EntityTemplateFromFile", typeof(XmlDbEntityTemplateFromFile)),]
        public readonly List<XmlDbEntityTemplate> Items = new List<XmlDbEntityTemplate>();

        #endregion Public Fields
    }
}
