using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Maps.Builders;
using OpenBreed.Common.Maps.Readers.MAP;

using OpenBreed.Common.Tiles;
using OpenBreed.Common.Actions;
using OpenBreed.Common.Sprites;

namespace OpenBreed.Common.Maps
{
    public class MapModel
    {

        #region Internal Constructors

        internal MapModel(MapBuilder builder)
        {
            Properties = builder.Properties;
            Mission = builder.Mission;
            Layout = builder.Body;
        }

        #endregion Internal Constructors

        #region Public Properties

        public MapLayoutModel Layout { get; }

        public MapMissionModel Mission { get; }

        public List<PaletteModel> Palettes { get; } = new List<PaletteModel>();

        public MapPropertiesModel Properties { get; }

        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        #endregion Public Properties
    }
}
