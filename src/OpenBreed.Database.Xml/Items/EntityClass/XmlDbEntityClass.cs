using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.EntityClasses;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Xml.Items.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.EntityClass
{
    [Serializable]
    public class XmlDbEntityClass : XmlDbEntry, IDbEntityClass
    {
        #region Public Constructors

        public XmlDbEntityClass()
        {
        }

        public XmlDbEntityClass(XmlDbEntityClass other) : base(other)
        {
            ParentId = other.ParentId;
        }

        #endregion Public Constructors

        #region Public Properties

        [XmlAttribute("ParentId")]
        public string ParentId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbEntityClass(this);

        #endregion Public Methods
    }
}