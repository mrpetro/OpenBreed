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
    [Description("Sprite atlas from Image"), Category("Appearance")]
    public class XmlDbSpriteAtlasFromImage : XmlDbSpriteAtlas, IDbSpriteAtlasFromImage
    {
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

        public override IDbEntry Copy()
        {
            return new XmlDbSpriteAtlasFromImage()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef
            };
        }

        public void ClearCoords()
        {
            XmlSprites.Clear();
        }

        public void AddCoords(int x, int y, int width, int height)
        {
            XmlSprites.Add(new XmlDbSpriteCoords() { X = x, Y = y, Width = width, Height = height });
        }
    }
}
