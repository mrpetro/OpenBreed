using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Props;

namespace OpenBreed.Common.XmlDatabase.Items.Props
{
    [Serializable]
    public class PropertySetDef : DatabaseItemDef, IPropSetEntity
    {

        #region Public Properties

        public long Id { get; set; }

        [XmlIgnore]
        public List<IPropertyEntity> Items
        {
            get
            {
                return PropertyDefs.OfType<IPropertyEntity>().ToList();
            }
        }

        [XmlArray("PropertyDefs")]
        [XmlArrayItem("PropertyDef")]
        public List<PropertyDef> PropertyDefs { get; } = new List<PropertyDef>();

        #endregion Public Properties
    }
}
