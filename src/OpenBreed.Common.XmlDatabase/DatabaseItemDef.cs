using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase
{
    public class DatabaseItemDef : IEntry
    {
        #region Public Properties

        [XmlAttribute]
        public string Description { get; set; }

        [XmlAttribute]
        public long Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"DbTableItem '{Name}'";
        }

        #endregion Public Methods
    }
}
