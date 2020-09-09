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
        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("EntityTemplateFromFile", typeof(XmlEntityTemplateFromFileEntry)),]
        public readonly List<XmlEntityTemplateEntry> Items = new List<XmlEntityTemplateEntry>();

        #endregion Public Fields
    }
}
