using OpenBreed.Common.Model.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Model.Maps.Builders
{
    public class MapBuilder
    {
        #region Internal Fields

        internal List<IMapDataBlock> Blocks = new List<IMapDataBlock>();
        internal byte[] Header;

        #endregion Internal Fields

        #region Public Methods

        public static MapBuilder NewMapModel()
        {
            return new MapBuilder();
        }

        public MapModel Build()
        {
            return new MapModel(this);
        }

        public MapBuilder SetHeader(byte[] header)
        {
            Header = header;

            return this;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AddBlock(IMapDataBlock dataBlock)
        {
            Blocks.Add(dataBlock);
        }

        #endregion Internal Methods
    }
}
