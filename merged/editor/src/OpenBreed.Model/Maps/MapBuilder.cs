using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Model.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Model.Maps
{
    public class MapBuilder
    {
        #region Public Fields

        //TODO: Make this one internal
        public List<IMapDataBlock> Blocks = new List<IMapDataBlock>();

        #endregion Public Fields

        #region Internal Fields

        internal byte[] Header;

        #endregion Internal Fields

        #region Private Fields

        private readonly IDrawingFactory drawingFactory;

        #endregion Private Fields

        #region Public Constructors

        public MapBuilder(IDrawingFactory drawingFactory)
        {
            this.drawingFactory = drawingFactory;
        }

        #endregion Public Constructors

        #region Internal Properties

        internal MapLayoutBuilder Layout { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public static MapBuilder NewMapModel(IDrawingFactory drawingFactory)
        {
            return new MapBuilder(drawingFactory);
        }

        public MapLayoutBuilder CreateLayout()
        {
            Layout = MapLayoutBuilder.NewMapLayoutModel(drawingFactory);
            return Layout;
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

        public void AddBlock(IMapDataBlock dataBlock)
        {
            Blocks.Add(dataBlock);
        }

        #endregion Public Methods
    }
}