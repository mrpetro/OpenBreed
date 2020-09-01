using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Actions;
using OpenBreed.Common.Builders.Maps;

namespace OpenBreed.Common.Model.Maps
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
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        #endregion Public Properties
    }
}
