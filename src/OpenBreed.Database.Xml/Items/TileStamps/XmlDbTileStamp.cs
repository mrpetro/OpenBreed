using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Database.Xml.Items.Tiles;
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
        #region Public Constructors

        public XmlDbTileStamp()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbTileStamp(XmlDbTileStamp other) : base(other)
        {
            Width = other.Width;
            Height = other.Height;
            CenterX = other.CenterX;
            CenterY = other.CenterY;
            XmlCells = other.XmlCells.Select(item => item.Copy()).Cast<XmlDbTileStampCell>().ToList();
        }

        #endregion Protected Constructors

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

        public IDbTileStampCell AddNewCell(int x, int y)
        {
            var newCell = new XmlDbTileStampCell()
            {
                X = x,
                Y = y
            };

            XmlCells.Add(newCell);

            return newCell;
        }

        public bool RemoveCell(IDbTileStampCell cell)
        {
            return XmlCells.Remove((XmlDbTileStampCell)cell);
        }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbTileStamp(this);

        #endregion Public Methods
    }

    [Serializable]
    public class XmlDbTileStampCell : IDbTileStampCell
    {
        #region Public Constructors

        public XmlDbTileStampCell()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbTileStampCell(XmlDbTileStampCell other)
        {
            X = other.X;
            Y = other.Y;
            TsId = other.TsId;
            TsTi = other.TsTi;
        }

        #endregion Protected Constructors

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

        public IDbTileStampCell Copy() => new XmlDbTileStampCell(this);

        #endregion Public Methods
    }
}