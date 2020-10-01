using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Model.Tiles;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Actions;

namespace OpenBreed.Model.Maps
{
    public class MapModel
    {
        public byte[] Header { get; }

        public List<IMapDataBlock> Blocks { get; }

        #region Internal Constructors

        internal MapModel(MapBuilder builder)
        {
            Header = builder.Header;
            Blocks = builder.Blocks;

            Palettes = new List<PaletteModel>();
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        public List<PaletteModel> Palettes { get; }

        public TileSetModel TileSet { get; set; }



        public ActionSetModel ActionSet { get; set; }

        #endregion Public Properties
    }
}
