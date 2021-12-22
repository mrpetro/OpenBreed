using OpenBreed.Model.Actions;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Model.Maps
{
    public class MapModel
    {
        #region Internal Constructors

        internal MapModel(MapBuilder builder)
        {
            Header = builder.Header;
            Blocks = builder.Blocks;

            Palettes = new List<PaletteModel>();
            Layout = builder.Layout.Build();
        }

        #endregion Internal Constructors

        #region Public Properties

        public byte[] Header { get; }

        public List<IMapDataBlock> Blocks { get; }

        public MapLayoutModel Layout { get; }

        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        public List<PaletteModel> Palettes { get; }

        public TileSetModel TileSet { get; set; }

        public ActionSetModel ActionSet { get; set; }

        public ActionModel GetAction(int actionValue)
        {
            return ActionSet.Items.FirstOrDefault(item => item.Id == actionValue);
        }

        #endregion Public Properties
    }
}