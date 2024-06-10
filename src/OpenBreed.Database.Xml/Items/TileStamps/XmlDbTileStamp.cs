using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.TileStamps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.TileStamps
{
    [Serializable]
    public class XmlDbTileStamp : XmlDbEntry, IDbTileStamp
    {
        #region Public Properties

        [XmlAttribute("Width")]
        public int Width { get; set; }

        [XmlAttribute("Height")]
        public int Height { get; set; }

        [XmlAttribute("CenterX")]
        public int CenterX { get; set; }

        [XmlAttribute("CenterY")]
        public int CenterY { get; set; }

        [XmlArray("Cells")]
        [XmlArrayItem(ElementName = "Cell")]
        public List<XmlDbTileStampCell> XmlCells { get; set; }

        [XmlIgnore]
        public ReadOnlyCollection<IDbTileStampCell> Cells
        {
            get
            {
                return new ReadOnlyCollection<IDbTileStampCell>(XmlCells.Cast<IDbTileStampCell>().ToList());
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy()
        {
            return new XmlDbTileStamp
            {
                Width = this.Width,
                Height = this.Height,
                CenterX = this.CenterX,
                CenterY = this.CenterY,
                XmlCells = this.XmlCells.Select(item => item.Copy()).Cast<XmlDbTileStampCell>().ToList()
            };
        }

        #endregion Public Methods
    }

    [Serializable]
    public class XmlDbTileStampCell : IDbTileStampCell
    {
        #region Public Properties

        [XmlAttribute("X")]
        public int X { get; set; }

        [XmlAttribute("Y")]
        public int Y { get; set; }

        [XmlAttribute("TsId")]
        public string TsId { get; set; }

        [XmlAttribute("TsTi")]
        public int TsTi { get; set; }

        #endregion Public Properties

        #region Public Methods

        public IDbTileStampCell Copy()
        {
            return new XmlDbTileStampCell
            {
                X = this.X,
                Y = this.Y,
                TsId = this.TsId,
                TsTi = this.TsTi
            };
        }

        #endregion Public Methods
    }
}