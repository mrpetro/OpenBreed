using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapBodyEditorM : BaseViewModel
    {
        #region Private Fields

        private MapBodyVM _currentMapBody;
        private Matrix _transformation;
        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public MapBodyEditorM(EditorVM root)
        {
            Root = root;

            PropertyChanged += MapBodyViewerVM_PropertyChanged;

            Transformation = new Matrix();
        }

        private void MapBodyViewerVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentMapBody):
                    Title = "Map body - " + CurrentMapBody.Map.Title;
                    break;
                default:
                    break;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public MapBodyVM CurrentMapBody
        {
            get { return _currentMapBody; }
            set { SetProperty(ref _currentMapBody, value); }
        }

        public EditorVM Root { get; private set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public Matrix Transformation
        {
            get { return _transformation; }
            set { SetProperty(ref _transformation, value); }
        }

        public float ZoomScale { get { return Transformation.Elements[0]; } }

        #endregion Public Properties

        #region Public Methods

        public void CenterView(float x, float y, float width, float height, float scale = 1.0f)
        {
            var newTransf = Transformation.Clone();
            newTransf.Reset();
            newTransf.Translate(width / 2, height / 2);
            newTransf.Scale(scale, scale);
            newTransf.Translate(-x, -y);
            Transformation = newTransf;
        }

        public void DrawView(Graphics gfx)
        {
            gfx.Transform = Transformation;
            RectangleF viewRect = gfx.ClipBounds;

            int tileSize = CurrentMapBody.Map.TileSize;
            int xFrom = CurrentMapBody.GetMapIndexX(viewRect.Left);
            int xTo = CurrentMapBody.GetMapIndexX(viewRect.Right);
            int yFrom = CurrentMapBody.GetMapIndexY(viewRect.Top);
            int yTo = CurrentMapBody.GetMapIndexY(viewRect.Bottom);

            var visibleLayers = CurrentMapBody.Layers.Where(item => item.IsVisible);

            foreach (var layer in visibleLayers)
                layer.DrawView(gfx, Rectangle.FromLTRB(xFrom, yTo, xTo, yFrom));


            //for (int xIndex = xFrom; xIndex <= xTo; xIndex++)
            //{
            //    for (int yIndex = yFrom; yIndex <= yTo; yIndex++)
            //    {
            //        var tile = GetCell(xIndex, yIndex);

            //        Map.Editor.TileSets.CurrentItem.DrawTile(gfx, tile.GfxId, xIndex * tileSize, yIndex * tileSize, tileSize);
            //        PropertySet.DrawProperty(gfx, tile.PropertyId, xIndex * tileSize, yIndex * tileSize, tileSize);
            //    }
            //}

            //PropertyInserter.DrawInsertion(gfx, tileSize);
        }

        public void FitViewToBody(float width, float height)
        {
            float scaleW = width / CurrentMapBody.MaxCoordX;
            float scaleH = height / CurrentMapBody.MaxCoordY;
            float scale = Math.Min(scaleW, scaleH);

            if (scale == 0.0f)
                scale = 1.0f;

            CenterView(CurrentMapBody.MaxCoordX / 2, CurrentMapBody.MaxCoordY / 2, width, height, scale);
        }

        public Point GetWorldCoords(Point viewCoords)
        {
            Matrix invMatrix = Transformation.Clone();
            invMatrix.Invert();

            Point[] clipPoints = new Point[1] { viewCoords };

            invMatrix.TransformPoints(clipPoints);

            return clipPoints[0];
        }

        public PointF GetWorldCoords(PointF viewCoords)
        {
            Matrix invMatrix = Transformation.Clone();
            invMatrix.Invert();

            PointF[] clipPoints = new PointF[1] { viewCoords };

            invMatrix.TransformPoints(clipPoints);

            return clipPoints[0];
        }

        public void ScrollViewBy(int deltaX, int deltaY)
        {
            var newTransf = Transformation.Clone();
            float currentScale = newTransf.Elements[0];
            newTransf.Translate(deltaX / currentScale, deltaY / currentScale);
            Transformation = newTransf;
        }

        public void SetScale(float scale)
        {
            var newTransf = Transformation.Clone();
            newTransf.Scale(scale, scale);
            Transformation = newTransf;
        }

        public void ZoomViewTo(Point location, float scale)
        {
            var newTransf = Transformation.Clone();

            Matrix invMatrix = newTransf.Clone();
            invMatrix.Invert();

            PointF[] t1Points = new PointF[1] { location };
            invMatrix.TransformPoints(t1Points);

            newTransf.Scale(scale, scale);

            invMatrix = newTransf.Clone();
            invMatrix.Invert();

            PointF[] t2Points = new PointF[1] { location };
            invMatrix.TransformPoints(t2Points);

            float offsetXDiff = t2Points[0].X - t1Points[0].X;
            float offsetYDiff = t2Points[0].Y - t1Points[0].Y;

            newTransf.Translate(offsetXDiff, offsetYDiff);
            Transformation = newTransf;
        }

        #endregion Public Methods
    }
}
