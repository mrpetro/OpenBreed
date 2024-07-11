using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Texts
{
    [Serializable]
    [Description("Text from MAP"), Category("Appearance")]
    public class XmlDbTextFromMap : XmlDbText, IDbTextFromMap
    {
        #region Public Constructors

        public XmlDbTextFromMap()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbTextFromMap(XmlDbTextFromMap other) : base(other)
        {
            MapRef = other.MapRef;
            BlockName = other.BlockName;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("MapRef")]
        public string MapRef { get; set; }

        [XmlElement("BlockName")]
        public string BlockName { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbTextFromMap(this);

        #endregion Public Methods
    }
}