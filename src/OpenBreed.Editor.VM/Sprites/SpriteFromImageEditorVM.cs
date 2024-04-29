using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using System;
using System.Drawing;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteFromImageEditorVM : BaseViewModel
    {
        #region Public Fields

        public IPen CurrentSpritePen { get; }
        public IBrush CurrentSpriteBrush { get; }
        public IPen ShadowSpritePen { get; }
        public IBrush ShadowSpriteBrush { get; }

        private readonly IDrawingFactory drawingFactory;

        #endregion Public Fields

        #region Private Fields

        private Size snapSize;

        private bool updateEnabled;
        private bool undoEnabled;

        private MyRectangle spriteRectangle;

        private string spriteRectangleText;

        #endregion Private Fields

        #region Public Constructors

        public SpriteFromImageEditorVM(SpriteSetFromImageEditorVM parent, IDrawingFactory drawingFactory)
        {
            Parent = parent;
            this.drawingFactory = drawingFactory;

            CurrentSpritePen = drawingFactory.CreatePen(MyColor.FromArgb(255, 255, 0, 0));
            CurrentSpriteBrush = drawingFactory.CreateSolidBrush(MyColor.FromArgb(50, 255, 0, 0));
            ShadowSpritePen = drawingFactory.CreatePen(MyColor.FromArgb(255, 0, 255, 0));
            ShadowSpriteBrush = drawingFactory.CreateSolidBrush(MyColor.FromArgb(25, 0, 255, 0));

            Parent.PropertyChanged += Owner_PropertyChanged;

            PropertyChanged += This_PropertyChanged;

            SnapSize = new Size(8, 8);

            ShowShadowRectangles = true;

            UpdateCommand = new Command(() => Update());
            UndoCommand = new Command(() => Undo());
        }

        #endregion Public Constructors

        #region Public Properties

        public ICommand UpdateCommand { get; }
        public ICommand UndoCommand { get; }

        public bool ShowShadowRectangles { get; set; }

        public bool UpdateEnabled
        {
            get { return updateEnabled; }
            set { SetProperty(ref updateEnabled, value); }
        }

        public bool UndoEnabled
        {
            get { return undoEnabled; }
            set { SetProperty(ref undoEnabled, value); }
        }

        public string SpriteRectangleText
        {
            get { return spriteRectangleText; }
            private set { SetProperty(ref spriteRectangleText, value); }
        }

        public Action CloseAction { get; set; }

        public SelectionRectangle SelectionRectangle { get; private set; }

        public Action RefreshAction { get; set; }

        public MyRectangle SpriteRectangle
        {
            get { return spriteRectangle; }
            private set { SetProperty(ref spriteRectangle, value); }
        }

        public SpriteSetFromImageEditorVM Parent { get; }

        public Size SnapSize
        {
            get { return snapSize; }
            set { SetProperty(ref snapSize, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Update()
        {
            Parent.CurrentSprite.SourceRectangle = SpriteRectangle;
            UpdateControls();
            Parent.UpdateSpriteImage(Parent.CurrentSprite, SpriteRectangle);
            Close();
        }

        public void Undo()
        {
            SpriteRectangle = Parent.CurrentSprite.SourceRectangle;
            UpdateControls();
            RefreshAction?.Invoke();
        }

        public void Cancel()
        {
            Close();
        }

        public void CursorUp(MyPoint location)
        {
            if (SelectionRectangle == null)
                return;

            location = GetSnapped(location);
            SelectionRectangle.SetFinish(location);
            SpriteRectangle = SelectionRectangle.GetRectangle();
            SelectionRectangle = null;
            RefreshAction?.Invoke();
            UpdateControls();
        }

        public void CursorDown(MyPoint location)
        {
            SelectionRectangle = new SelectionRectangle();
            location = GetSnapped(location);
            SelectionRectangle.SetStart(location);
            SpriteRectangle = SelectionRectangle.GetRectangle();

            RefreshAction?.Invoke();
        }

        public void CursorMove(MyPoint location)
        {
            if (SelectionRectangle == null)
                return;

            location = GetSnapped(location);
            SelectionRectangle.Update(location);
            SpriteRectangle = SelectionRectangle.GetRectangle();
            RefreshAction?.Invoke();
        }

        public void DrawSpriteEditorView(IDrawingContext gfx)
        {
            if (Parent.SourceImage == null)
                return;

            int width = Parent.SourceImage.Width;
            int height = Parent.SourceImage.Height;

            gfx.DrawImage(Parent.SourceImage, 0, 0, width, height);

            if (ShowShadowRectangles)
                DrawShadowRectangles(gfx);

            gfx.FillRectangle(CurrentSpriteBrush, SpriteRectangle);
            gfx.DrawRectangle(CurrentSpritePen, SpriteRectangle);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Close()
        {
            CloseAction?.Invoke();
        }

        #endregion Internal Methods

        #region Private Methods

        private void DrawShadowRectangles(IDrawingContext gfx)
        {
            for (int i = 0; i < Parent.Items.Count; i++)
            {
                if (i == Parent.CurrentSpriteIndex)
                    continue;

                var item = Parent.Items[i];
                var r = ((SpriteFromImageVM)item).SourceRectangle;
                gfx.FillRectangle(ShadowSpriteBrush, r);
                gfx.DrawRectangle(ShadowSpritePen, r);
            }
        }

        private void This_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SpriteRectangle):
                    SpriteRectangleText = $"X: {SpriteRectangle.X} Y: {SpriteRectangle.Y} Width: {SpriteRectangle.Width} Height: {SpriteRectangle.Height}";
                    break;

                default:
                    break;
            }
        }

        private void UpdateControls()
        {
            var isModified = SpriteRectangle != Parent.CurrentSprite.SourceRectangle;
            UpdateEnabled = isModified && (SpriteRectangle.Width != 0 && SpriteRectangle.Height != 0);
            UndoEnabled = isModified;
        }

        private void Owner_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Parent.CurrentSpriteIndex):
                    Restore(Parent.CurrentSpriteIndex);
                    break;

                default:
                    break;
            }
        }

        private MyPoint GetSnapped(MyPoint point)
        {
            var x = SnapSize.Width * (int)Math.Round((double)point.X / SnapSize.Width);
            var y = SnapSize.Height * (int)Math.Round((double)point.Y / SnapSize.Height);
            return new MyPoint(x, y);
        }

        private void Restore(int spriteIndex)
        {
            if (spriteIndex < 0)
                return;

            SpriteRectangle = Parent.CurrentSprite.SourceRectangle;
            UpdateControls();
            RefreshAction?.Invoke();
        }

        #endregion Private Methods

    }
}