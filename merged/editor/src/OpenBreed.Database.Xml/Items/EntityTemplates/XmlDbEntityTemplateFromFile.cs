using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
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
    [Description("Entity template from file"), Category("Appearance")]
    public class XmlDbEntityTemplateFromFile : XmlDbEntityTemplate, IDbEntityTemplateFromFile
    {
        #region Public Constructors

        public XmlDbEntityTemplateFromFile()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbEntityTemplateFromFile(XmlDbEntityTemplateFromFile other) : base(other)
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbEntityTemplateFromFile(this);

        #endregion Public Methods
    }
}