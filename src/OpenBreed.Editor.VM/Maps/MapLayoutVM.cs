using OpenBreed.Common.Maps;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Maps.Helpers;
using OpenBreed.Editor.VM.Props;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OpenBreed.Editor.VM.Maps.Layers;
using OpenBreed.Common.Tiles;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Common.Actions;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapLayoutVM : BaseViewModel
    {

        #region Private Fields

        private Size _size;

        #endregion Private Fields

        #region Public Constructors

        public MapLayoutVM(MapVM owner)
        {
            Owner = owner;
            Layers = new BindingList<MapLayerBaseVM>();

            //TilesInserter = new TilesInserter(this, Map.Project.Root.TileSets.CurrentItem.Selector);
            //PropertyInserter = new PropertyInserter(this, Map.Root.PropSets.Selector);

        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<MapLayerBaseVM> Layers { get; }
        public float MaxCoordX { get; private set; }
        public float MaxCoordY { get; private set; }
        public MapVM Owner { get; }

        public Size Size
        {
            get { return _size; }
            set { SetProperty(ref _size, value); }
        }

        #endregion Public Properties

        //public TilesInserter TilesInserter { get; private set; }

        #region Public Methods

        public int GetMapIndexX(float xCoord)
        {
            int xIndex = (int)(xCoord / Owner.TileSize);

            if (xIndex < 0)
                xIndex = 0;
            else if (xIndex > Size.Width - 1)
                xIndex = Size.Width - 1;

            return xIndex;
        }

        public int GetMapIndexY(float yCoord)
        {
            int yIndex = (int)(yCoord / Owner.TileSize);

            if (yIndex < 0)
                yIndex = 0;
            else if (yIndex > Size.Height - 1)
                yIndex = Size.Height - 1;

            return yIndex;
        }

        //public MapCellModel GetCell(int x, int y)
        //{
        //    return CurrentMapBody.Cells[x, y];
        //}


        public void Resize(int newSizeX, int newSizeY)
        {
            //m_CurrentMapBody.Resize(newSizeX, newSizeY);
        }

        public void ResizeOld(int newSizeX, int newSizeY)
        {
            //var mapResizeOperation = new MapResizeOperation(Size.Width, Size.Height, newSizeX, newSizeY);
            //Map.Commands.ExecuteCommand(new CmdResize(this, mapResizeOperation));
        }

        public void SetTileGfx(int x, int y, int gfxId)
        {
            //CurrentMapBody.SetTileGfx(x, y, gfxId);

            Owner.IsModified = true;
        }

        public void SetTileProperty(int x, int y, int propertyId)
        {
            //CurrentMapBody.SetTileProperty(x, y, propertyId);

            Owner.IsModified = true;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void ConnectEvents()
        {
        }

        internal Point GetIndexCoords(Point point)
        {
            return new Point(point.X / Owner.TileSize, point.Y / Owner.TileSize);
        }

        internal void FromModel(MapLayoutModel layout)
        {
            Size = layout.Size;

            Layers.UpdateAfter(() => 
            {
                Layers.Clear();

                foreach (var layer in layout.Layers)
                    AppendLayer(layer);
            });

            MaxCoordX = Size.Width * Owner.TileSize;
            MaxCoordY = Size.Height * Owner.TileSize;
        }

        #endregion Internal Methods

        #region Private Methods

        private void AppendLayer(IMapLayerModel layer)
        {
            MapLayerBaseVM newLayerVM = null;

            if (layer.Name == "GFX")
                newLayerVM = new MapLayerGfxVM(this);
            else if (layer.Name == "PROP")
                newLayerVM = new MapLayerActionVM(this);

            newLayerVM.Restore(layer);
            Layers.Add(newLayerVM);
        }

        #endregion Private Methods

    }
}