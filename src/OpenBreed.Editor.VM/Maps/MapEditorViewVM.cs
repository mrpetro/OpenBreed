﻿using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Tools;
using OpenBreed.Editor.VM.Renderer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorViewVM : BaseViewModel, IScrollableVM, IZoomableVM
    {

        #region Private Fields

        private MapLayoutVM _layout;
        private string _title;
        private Matrix _transformation;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorViewVM(MapEditorVM parent)
        {
            Parent = parent;

            Transformation = new Matrix();
            Cursor = new MapViewCursorVM();

            Cursor.CalculateWorldCoordsFunc = GetWorldSnapCoords;
            Cursor.CalculateWorldIndexCoordsFunc = GetIndexCoords;

            Cursor.PropertyChanged += Cursor_PropertyChanged;

            PropertyChanged += MapBodyViewerVM_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public MapViewCursorVM Cursor { get; }
        public MapLayoutVM Layout
        {
            get { return _layout; }
            set { SetProperty(ref _layout, value); }
        }

        public MapEditorVM Parent { get; }

        public Action RefreshAction { get; set; }

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

        public void FitViewToBody(float width, float height)
        {
            float scaleW = width / Layout.MaxCoordX;
            float scaleH = height / Layout.MaxCoordY;
            float scale = Math.Min(scaleW, scaleH);

            if (scale == 0.0f)
                scale = 1.0f;

            CenterView(Layout.MaxCoordX / 2, Layout.MaxCoordY / 2, width, height, scale);
        }

        public Point GetIndexCoords(Point point)
        {
            return new Point(point.X / 16, point.Y / 16);
        }

        public Point GetWorldSnapCoords(Point point)
        {
            var worldCoords = GetWorldCoords(point);

            return new Point((worldCoords.X /  16) * 16, (worldCoords.Y / 16) * 16);
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

        public void Refresh()
        {
            RefreshAction?.Invoke();
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
            var newScale = scale / newTransf.Elements[0];
            newTransf.Scale(newScale, newScale);
            Transformation = newTransf;
        }

        public void ZoomViewTo(Point location, float scale)
        {
            var newTransf = Transformation.Clone();

            Matrix invMatrix = newTransf.Clone();
            invMatrix.Invert();

            PointF[] t1Points = new PointF[1] { location };
            invMatrix.TransformPoints(t1Points);

            var toScale = scale / newTransf.Elements[0];
            newTransf.Scale(toScale, toScale);

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

        #region Private Methods

        private Point CalculateWorldPosition(Point viewPosition)
        {
            var inverted = Transformation.Clone();
            //inverted.Invert();

            var list = new Point[] { viewPosition };

            inverted.TransformPoints(list);
            return list[0];
        }

        private void Cursor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Refresh();
        }
        private void MapBodyViewerVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Layout):
                    Title = "Map body - " + Layout.Owner.Title;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}
