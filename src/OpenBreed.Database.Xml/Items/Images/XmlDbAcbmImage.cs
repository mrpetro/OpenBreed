using OpenBreed.Database.Xml.Items.Assets;
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
    [Description("ACBM image"), Category("Appearance")]
    public class XmlDbAcbmImage : XmlDbImage, IDbAcbmImage
    {
        #region Public Constructors

        public XmlDbAcbmImage()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbAcbmImage(XmlDbAcbmImage other) : base(other)
        {
            DataRef = other.DataRef;
            Width = other.Width;
            Height = other.Height;
            BitPlanesNo = other.BitPlanesNo;
            PaletteMode = other.PaletteMode;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlElement("Width")]
        public int Width { get; set; }

        [XmlElement("Height")]
        public int Height { get; set; }

        [XmlElement("BitPlanesNo")]
        public int BitPlanesNo { get; set; }

        [XmlElement("PaletteMode")]
        public string PaletteMode { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbAcbmImage(this);

        #endregion Public Methods
    }
}