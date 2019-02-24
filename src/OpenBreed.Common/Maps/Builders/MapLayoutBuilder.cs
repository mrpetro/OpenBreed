using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Maps.Builders
{
    public class MapLayoutBuilder
    {
        internal List<IMapLayerModel> Layers = new List<IMapLayerModel>();
        internal Size Size;

        public static MapLayoutBuilder NewLayoutModel()
        {
            return new MapLayoutBuilder();
        }

        public MapLayoutModel Build()
        {
            return new MapLayoutModel(this);
        }

        public MapLayoutBuilder SetSize(int sizeX, int sizeY)
        {
            Size = new Size(sizeX, sizeY);

            return this;
        }

        //public MapBodyBuilder CreateCells(int sizeX, int sizeY)
        //{
        //    Cells = new MapCellModel[sizeX, sizeY];

        //    for (int indexY = 0; indexY < sizeY; indexY++)
        //        for (int indexX = 0; indexX < sizeX; indexX++)
        //            Cells[indexX, indexY] = MapCellBuilder.NewCellModel().SetIndexXY(indexX, indexY).Build();

        //    return this;
        //}

        //public MapBodyBuilder SetTile(int indexX, int indexY, int gfxId, int typeId)
        //{
        //    if (Cells == null)
        //        throw new Exception("Tile matrix not created first!");

        //    if (indexX >= Cells.GetLength(0))
        //        throw new Exception("Tile indexX is bigger than tile matrix size!");

        //    if (indexY >= Cells.GetLength(1))
        //        throw new Exception("Tile indexY is bigger than tile matrix size!");

        //    MapCellModel tile = Cells[indexX, indexY];

        //    tile.GfxId = gfxId;
        //    tile.PropertyId = typeId;

        //    return this;
        //}

        //public MapBodyBuilder SetTile(MapCellModel tileModel)
        //{
        //    if (Cells == null)
        //        throw new Exception("Tile matrix not created first!");

        //    if (tileModel.IndexX >= Cells.GetLength(0))
        //        throw new Exception("Tile indexX is bigger than tile matrix size!");

        //    if (tileModel.IndexY >= Cells.GetLength(1))
        //        throw new Exception("Tile indexY is bigger than tile matrix size!");

        //    Cells[tileModel.IndexX, tileModel.IndexY] = tileModel;
        //    return this;
        //}

        public void AddLayer(IMapLayerModel mapBodyLayerModel)
        {
            Layers.Add(mapBodyLayerModel);
        }
    }
}
