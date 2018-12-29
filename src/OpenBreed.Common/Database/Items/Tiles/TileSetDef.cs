using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Items.Tiles
{
    [Serializable]
    public class TileSetDef : DatabaseItemDef, ITileSetEntity
    {
        #region Public Properties

        [XmlIgnore]
        public IFormatEntity Format { get; set; }

        [XmlElement("Format")]
        public FormatDef FormatDef
        {
            get
            {
                return (FormatDef)Format;
            }

            set
            {
                Format = value;
            }
        }

        public long Id { get; set; }
        public string SourceRef { get; set; }

        #endregion Public Properties
    }
}
