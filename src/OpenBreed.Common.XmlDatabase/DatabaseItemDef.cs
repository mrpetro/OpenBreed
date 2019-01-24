using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase
{
    public abstract class DatabaseItemDef : IEntry
    {
        #region Public Properties

        [XmlAttribute]
        public string Description { get; set; }

        [XmlAttribute]
        public string Id { get; set; }

        public abstract IEntry Copy();

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"DbTableItem '{Id}'";
        }

        #endregion Public Methods
    }
}
