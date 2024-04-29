using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Renderer;
using OpenBreed.Editor.VM.Tools;
using OpenBreed.Model.Maps;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorViewVM : BaseViewModel, IScrollableVM, IZoomableVM
    {
        #region Private Fields

        private ViewRenderer renderer;
        private readonly IDrawingFactory drawingFactory;
        private string _title;
        private IMatrix _transformation;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorViewVM(
            MapEditorVM parent,
            ViewRenderer renderer,
            IRenderTarget renderTarget,
            IDrawingFactory drawingFactory)
        {
            Parent = parent;
            this.renderer = renderer;
            RenderTarget = renderTarget;
            this.drawingFactory = drawingFactory;
            Transformation = drawingFactory.CreateMatrix();
            Cursor = new MapViewCursorVM(Parent, drawingFactory);

            Cursor.ToWorldCoordsFunc = ToWorldCoords;
            Cursor.PropertyChanged += Cursor_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public IRenderTarget RenderTarget { get; }
        public MapViewCursorVM Cursor { get; }

        public MapLayoutModel Layout => Parent.Layout;

        public MapEditorVM Parent { get; }

        public Action RefreshAction { get; set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public IMatrix Transformation
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

        public void Resize(int width, int height)
        {
            RenderTarget.Resize(width, height);
        }

        public MyPoint ToWorldCoords(MyPoint viewCoords)
        {
            var invMatrix = Transformation.Clone();
            invMatrix.Invert();

            var clipPoints = new MyPoint[1] { viewCoords };

            invMatrix.TransformPoints(clipPoints);

            var x = clipPoints[0].X;
            var y = clipPoints[0].Y;

            return new MyPoint(x, y);
        }

        public MyPointF ToWorldCoords(MyPointF viewCoords)
        {
            var invMatrix = Transformation.Clone();
            invMatrix.Invert();

            MyPointF[] clipPoints = new MyPointF[1] { viewCoords };

            invMatrix.TransformPoints(clipPoints);

            var x = clipPoints[0].X; 
            var y =- clipPoints[0].Y;

            return new MyPointF(x, y);
        }

        public void Render(IDrawingContext graphics)
        {
            if (Parent.Model is null)
                return;

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

        public void ZoomViewTo(MyPointF location, float scale)
        {
            var newTransf = Transformation.Clone();

            var invMatrix = newTransf.Clone();
            invMatrix.Invert();

            var t1Points = new MyPointF[1] { location };
            invMatrix.TransformPoints(t1Points);

            var toScale = scale / newTransf.Elements[0];
            newTransf.Scale(toScale, toScale);

            invMatrix = newTransf.Clone();
            invMatrix.Invert();

            var t2Points = new MyPointF[1] { location };
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

        private MyPoint CalculateWorldPosition(MyPoint viewPosition)
        {
            var inverted = Transformation.Clone();
            //inverted.Invert();

            var list = new MyPoint[] { viewPosition };

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