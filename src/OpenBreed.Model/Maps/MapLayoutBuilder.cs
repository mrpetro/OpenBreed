using System;
using System.Collections.Generic;

namespace OpenBreed.Model.Maps
{
    public class MapLayoutBuilder
    {
        #region Private Constructors

        private MapLayoutBuilder()
        {
            Layers = new List<MapLayerBuilder>();
        }

        #endregion Private Constructors

        #region Internal Properties


        internal int Width { get; private set; }
        internal int Height { get; private set; }

        internal List<MapLayerBuilder> Layers { get; }

        #endregion Internal Properties

        #region Public Methods

        public static MapLayoutBuilder NewMapLayoutModel()
        {
            return new MapLayoutBuilder();
        }

        public void SetSize(int width, int height)
        {
            if (width == 0 || height == 0)
                throw new InvalidOperationException("Width and Height must have non zero positive values");

            Width = width;
            Height = height;
        }



        public MapLayoutModel Build()
        {
            return new MapLayoutModel(this);
        }

        public MapLayerBuilder AddLayer(MapLayerType layerType)
        {
            if (Width == 0 || Height == 0)
                throw new InvalidOperationException("Width and Height must have non zero positive values");

            var layerBuilder = new MapLayerBuilder(Width, Height, layerType);
            Layers.Add(layerBuilder);
            return layerBuilder;
        }

        #endregion Public Methods
    }
}