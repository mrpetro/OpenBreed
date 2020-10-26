using OpenBreed.Model.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Model.Maps
{
    public class MapBuilder
    {
        #region Internal Fields

        //TODO: Make this one internal
        public List<IMapDataBlock> Blocks = new List<IMapDataBlock>();
        internal byte[] Header;

        #endregion Internal Fields

        internal MapLayoutBuilder Layout { get; private set; }

        #region Public Methods

        public MapLayoutBuilder CreateLayout()
        {
            Layout = MapLayoutBuilder.NewMapLayoutModel();
            return Layout;
        }

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

        public void AddBlock(IMapDataBlock dataBlock)
        {
            Blocks.Add(dataBlock);
        }

        #endregion Internal Methods
    }
}
