using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Model.Maps
{
    public interface IMapLayoutModel
    {
        #region Public Properties

        int CellSize { get; }
        int Height { get; }
        int Width { get; }

        MyRectangleF Bounds { get; }

        #endregion Public Properties

        #region Public Methods

        int GetCellValue(int layerIndex, int x, int y);

        void SetCellValue(int layerIndex, int x, int y, int value);

        MyRectangle IndexBounds { get; }

        int GetLayerIndex(MapLayerType layerType);

        MyPoint GetIndexPoint(MyPoint point);

        #endregion Public Methods
    }
}