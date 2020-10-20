using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Renderer;
using OpenBreed.Editor.VM.Tools;
using OpenBreed.Model.Maps;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorViewVM : BaseViewModel, IScrollableVM, IZoomableVM
    {
        #region Private Fields

        private ViewRenderer renderer;
        private string _title;
        private Matrix _transformation;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorViewVM(MapEditorVM parent, ViewRenderer renderer, RenderTarget renderTarget)
        {
            Parent = parent;
            this.renderer = renderer;
            RenderTarget = renderTarget;

            Transformation = new Matrix();
            Cursor = new MapViewCursorVM();

            Cursor.CalculateWorldCoordsFunc = GetWorldSnapCoords;
            Cursor.CalculateWorldIndexCoordsFunc = GetIndexCoords;

            Cursor.PropertyChanged += Cursor_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public RenderTarget RenderTarget { get; }
        public MapViewCursorVM Cursor { get; }

        public MapLayoutModel Layout => Parent.Layout;

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
            float scaleW = width / Layout.Bounds.Width;
            float scaleH = height / Layout.Bounds.Height;
            float scale = Math.Min(scaleW, scaleH);

            if (scale == 0.0f)
                scale = 1.0f;

            CenterView(Layout.Bounds.Width / 2, Layout.Bounds.Height / 2, width, height, scale);
        }

        public Point GetIndexCoords(Point point)
        {
            return new Point(point.X / 16, point.Y / 16);
        }

        public void Resize(int width, int height)
        {
            RenderTarget.Resize(width, height);
        }

        public Point GetWorldSnapCoords(Point point)
        {
            var worldCoords = GetWorldCoords(point);

            return new Point((worldCoords.X / 16) * 16, (worldCoords.Y / 16) * 16);
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

        public void Render(Graphics graphics)
        {
            renderer.Render(this);
            RenderTarget.Flush(graphics);
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

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(Layout):
                    //Title = "Map body - " + Layout.Parent.Title;
                    Refresh();
                    break;
                case nameof(Transformation):
                    Refresh();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

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

        #endregion Private Methods
    }
}