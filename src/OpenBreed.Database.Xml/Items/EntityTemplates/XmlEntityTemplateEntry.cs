using OpenBreed.Database.Interface.Items.EntityTemplates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.EntityTemplates
{
    [Serializable]

    public abstract class XmlEntityTemplateEntry : XmlDbEntry, IEntityTemplateEntry
    {
        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        #endregion Public Properties
    }
}
