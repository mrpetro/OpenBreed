//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;
//using System.IO;
//using OpenBreed.Common.Maps.Builders;

//namespace OpenBreed.Common.Maps
//{
//    public class MapLayoutModel
//    {
//        private IMapLayerModel[] _layers;

//        #region Internal Constructors

//        internal MapLayoutModel(MapLayoutBuilder builder)
//        {
//            Layers = builder.Layers.ToArray();
//            Size = builder.Size;
//            Cells = builder.Cells;
//        }

//        #endregion Internal Constructors

//        #region Public Properties

//        public IMapLayerModel[] Layers
//        {
//            get { return _layers; }
//            private set
//            {
//                _layers = value;

//                foreach (var layer in _layers)
//                    layer.Owner = this;
//            }
//        }

//        public Size Size { get; private set; }

//        #endregion Public Properties

//        #region Public Methods

//        public void Resize(int newSizeX, int newSizeY)
//        {
//            if (newSizeX <= 0 || newSizeY <= 0)
//                throw new Exception("Resize: Invalid input size!");

//            //MapCellModel[,] oldMapTiles = Cells;
//            //int oldSizeX = SizeX;
//            //int oldSizeY = SizeY;

//            //MapCellModel[,] newMapTiles = new MapCellModel[newSizeX, newSizeY];

//            //for (int yIndex = 0; yIndex < newSizeY; yIndex++)
//            //{
//            //    for (int xIndex = 0; xIndex < newSizeX; xIndex++)
//            //    {
//            //        MapCellModel oldTile = null;
//            //        if (xIndex >= oldSizeX || yIndex >= oldSizeY)
//            //            oldTile = MapCellBuilder.NewCellModel().SetIndexXY(xIndex, yIndex).Build();
//            //        else
//            //            oldTile = oldMapTiles[xIndex, yIndex];

//            //        newMapTiles[xIndex, yIndex] = oldTile;
//            //    }
//            //}

//            //Cells = newMapTiles;
//        }

//        #endregion Public Methods
//    }
//}
