using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml
{
    public abstract class XmlDbEntry : IDbEntry
    {
        #region Public Properties

        [XmlAttribute]
        public string Description { get; set; }

        [XmlAttribute]
        public string Id { get; set; }

        public abstract IDbEntry Copy();

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"DbTableItem '{Id}'";
        }

        #endregion Public Methods
    }
}
