using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Xml.Items.Sounds;
using System.Threading.Channels;

namespace OpenBreed.Database.Xml.Items.Images
{
    [Serializable]
    [Description("IFF image"), Category("Appearance")]
    public class XmlDbIffImage : XmlDbImage, IDbIffImage
    {
        #region Public Constructors

        public XmlDbIffImage()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbIffImage(XmlDbIffImage other) : base(other)
        {
            DataRef = other.DataRef;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbIffImage(this);

        #endregion Public Methods
    }
}