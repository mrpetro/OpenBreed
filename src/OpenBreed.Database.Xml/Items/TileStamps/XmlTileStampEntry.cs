﻿using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.TileStamps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.TileStamps
{
    [Serializable]
    public class XmlTileStampEntry : XmlDbEntry, ITileStampEntry
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
        public List<XmlTileStampCell> XmlCells { get; set; }

        [XmlIgnore]
        public ReadOnlyCollection<ITileStampCell> Cells
        {
            get
            {
                return new ReadOnlyCollection<ITileStampCell>(XmlCells.Cast<ITileStampCell>().ToList());
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override IEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }

    [Serializable]
    public class XmlTileStampCell : ITileStampCell
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
    }
}