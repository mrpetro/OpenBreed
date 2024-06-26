using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sprites;
using System;
using System.Collections.Generic;
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
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbSpriteAtlasFromSpr(this);

        #endregion Public Methods
    }
}