using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Xml.Items.DataSources;
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
    public abstract class XmlDbEntityTemplate : XmlDbEntry, IDbEntityTemplate
    {
        #region Protected Constructors

        protected XmlDbEntityTemplate()
        {
        }

        protected XmlDbEntityTemplate(XmlDbEntityTemplate other) : base(other)
        {
            DataRef = other.DataRef;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        #endregion Public Properties
    }
}