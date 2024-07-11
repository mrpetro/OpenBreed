using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Xml.Items.Scripts;
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
    [Description("Sprite atlas from Image"), Category("Appearance")]
    public class XmlDbSpriteAtlasFromImage : XmlDbSpriteAtlas, IDbSpriteAtlasFromImage
    {
        #region Public Constructors

        public XmlDbSpriteAtlasFromImage()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbSpriteAtlasFromImage(XmlDbSpriteAtlasFromImage other) : base(other)
        {
            DataRef = other.DataRef;
            XmlSprites = other.XmlSprites.Select(item => item.Copy()).Cast<XmlDbSpriteCoords>().ToList();
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlArray("Sprites")]
        [XmlArrayItem(ElementName = "SpriteCoords")]
        public List<XmlDbSpriteCoords> XmlSprites { get; set; }

        [XmlIgnore]
        public ReadOnlyCollection<IDbSpriteCoords> Sprites
        {
            get
            {
                return new ReadOnlyCollection<IDbSpriteCoords>(XmlSprites.Cast<IDbSpriteCoords>().ToList());
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbSpriteAtlasFromImage(this);

        public void ClearCoords()
        {
            XmlSprites.Clear();
        }

        public void AddCoords(int x, int y, int width, int height)
        {
            XmlSprites.Add(new XmlDbSpriteCoords() { X = x, Y = y, Width = width, Height = height });
        }

        #endregion Public Methods
    }
}