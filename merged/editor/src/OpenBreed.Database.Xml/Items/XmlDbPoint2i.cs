using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Xml.Items.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items
{
    public class XmlDbPoint2i : IDbPoint2i
    {
        #region Public Constructors

        public XmlDbPoint2i()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbPoint2i(XmlDbPoint2i other)
        {
            X = other.X;
            Y = other.Y;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlAttribute("X")]
        public int X { get; set; }

        [XmlAttribute("Y")]
        public int Y { get; set; }

        #endregion Public Properties

        #region Public Methods

        public IDbPoint2i Copy() => new XmlDbPoint2i(this);

        #endregion Public Methods
    }
}