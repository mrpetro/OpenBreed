using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sprites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Sprites
{
    [Serializable]
    [Description("Sprite atlas from SPR"), Category("Appearance")]
    public class XmlDbSpriteAtlasFromSpr : XmlDbSpriteAtlas, IDbSpriteAtlasFromSpr
    {
        #region Public Constructors

        public XmlDbSpriteAtlasFromSpr()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbSpriteAtlasFromSpr(XmlDbSpriteAtlasFromSpr other) : base(other)
        {
            DataRef = other.DataRef;
            XmlSizes = other.XmlSizes.Select(item => (XmlDbPoint2i)item.Copy()).ToList();
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlArray("Sizes")]
        [XmlArrayItem(ElementName = "Size")]
        public List<XmlDbPoint2i> XmlSizes { get; set; }

        [XmlIgnore]
        public ReadOnlyCollection<IDbPoint2i> Sizes
        {
            get
            {
                return new ReadOnlyCollection<IDbPoint2i>(XmlSizes.Cast<IDbPoint2i>().ToList());
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbSpriteAtlasFromSpr(this);

        #endregion Public Methods
    }
}