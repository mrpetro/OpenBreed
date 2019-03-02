using OpenBreed.Common.Maps;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Maps.Helpers;
using OpenBreed.Editor.VM.Actions;
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
using OpenBreed.Common.Maps.Builders;
using OpenBreed.Common;
using OpenBreed.Common.Maps.Blocks;

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
            Layers.ListChanged += (s, e) => OnPropertyChanged(nameof(Layers));
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

        internal void ToMap(MapModel mapModel)
        {
            var xblk = mapModel.Blocks.OfType<MapUInt32Block>().FirstOrDefault(item => item.Name == "XBLK");
            var yblk = mapModel.Blocks.OfType<MapUInt32Block>().FirstOrDefault(item => item.Name == "YBLK");
            xblk.Value = (UInt32)Size.Width;
            yblk.Value = (UInt32)Size.Height;

            var bodyBlock = mapModel.Blocks.OfType<MapBodyBlock>().FirstOrDefault(item => item.Name == "BODY");

            var gfxLayer = Layers.OfType<MapLayerGfxVM>().FirstOrDefault();
            var actionLayer = Layers.OfType<MapLayerActionVM>().FirstOrDefault();

            for (int i = 0; i < bodyBlock.Cells.Length; i++)
            {
                var cell = bodyBlock.Cells[i];
                cell.ActionId = actionLayer.GetCell(i);
                cell.GfxId = gfxLayer.GetCell(i).TileId;
                bodyBlock.Cells[i] = cell;
            }
        }

        internal void FromMap(MapModel mapModel)
        {
            int sizeX = (int)mapModel.Blocks.OfType<MapUInt32Block>().FirstOrDefault(item => item.Name == "XBLK").Value;
            int sizeY = (int)mapModel.Blocks.OfType<MapUInt32Block>().FirstOrDefault(item => item.Name == "YBLK").Value;


            Size = new Size(sizeX, sizeY);

            var bodyBlock = mapModel.Blocks.OfType<MapBodyBlock>().FirstOrDefault(item => item.Name == "BODY");

            Layers.UpdateAfter(() =>
            {
                Layers.Clear();

                var gfxLayerVM = new MapLayerGfxVM(this);
                gfxLayerVM.Restore(bodyBlock);
                Layers.Add(gfxLayerVM);
                var actionLayerVM = new MapLayerActionVM(this);
                actionLayerVM.Restore(bodyBlock);
                Layers.Add(actionLayerVM);
            });

            MaxCoordX = Size.Width * Owner.TileSize;
            MaxCoordY = Size.Height * Owner.TileSize;
        }

        #endregion Internal Methods


    }
}