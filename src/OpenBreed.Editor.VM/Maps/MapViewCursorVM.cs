﻿using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Maps;
using System;
using System.Drawing;

namespace OpenBreed.Editor.VM.Maps
{
    public enum CursorButtons
    {
        None = 0,
        Left = 1,
        Right = 2,
        Middle = 4
    }

    public enum CursorActions
    {
        Hover,
        Leave,
        Move,
        Click,
        Up,
        Down
    }

    public class MapViewCursorVM : BaseViewModel
    {
        #region Public Fields

        public Action<MapViewCursorVM> UpdateAction;

        #endregion Public Fields

        #region Private Fields

        private CursorActions action;
        private CursorButtons buttons;
        private MyPoint viewCoords;
        private bool visible;
        private MyPoint worldCoords;
        private MyPoint worldIndexCoords;
        private string info;
        private readonly IDrawingFactory drawingFactory;

        #endregion Private Fields

        #region Public Constructors

        public MapViewCursorVM(
            MapEditorVM parent,
            IDrawingFactory drawingFactory)
        {
            Parent = parent;
            this.drawingFactory = drawingFactory;
        }

        #endregion Public Constructors

        #region Public Properties

        public MapLayoutModel Layout => Parent.Layout;
        public MapEditorVM Parent { get; }

        public Func<MyPoint, MyPoint> ToWorldCoordsFunc { get; set; }

        public CursorActions Action
        {
            get { return action; }
            private set { SetProperty(ref action, value); }
        }

        public CursorButtons Buttons
        {
            get { return buttons; }
            private set { SetProperty(ref buttons, value); }
        }

        public MyPoint ViewCoords
        {
            get { return viewCoords; }
            private set { SetProperty(ref viewCoords, value); }
        }

        public bool Visible
        {
            get { return visible; }
            private set { SetProperty(ref visible, value); }
        }

        public MyPoint WorldCoords
        {
            get { return worldCoords; }
            set { SetProperty(ref worldCoords, value); }
        }

        public MyPoint WorldSnapCoords
        {
            get
            {
                return new MyPoint(WorldIndexCoords.X * Layout.CellSize, WorldIndexCoords.Y * Layout.CellSize);
            }
        }

        public string Info
        {
            get { return info; }
            set { SetProperty(ref info, value); }
        }

        public MyPoint WorldIndexCoords
        {
            get { return worldIndexCoords; }
            set { SetProperty(ref worldIndexCoords, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Hover()
        {
            if (Parent.Model is null)
                return;

            Visible = true;
            Action = CursorActions.Hover;
            UpdateAction?.Invoke(this);
        }

        public void Leave()
        {
            if (Parent.Model is null)
                return;

            Visible = false;
            Action = CursorActions.Leave;
            UpdateAction?.Invoke(this);
        }

        public void Move(CursorButtons buttons, MyPoint location)
        {
            if (Parent.Model is null)
                return;

            Buttons = buttons;
            ViewCoords = location;
            Action = CursorActions.Move;
            UpdateAction?.Invoke(this);
        }

        public void Click(CursorButtons buttons, MyPoint location)
        {
            if (Parent.Model is null)
                return;

            Buttons = buttons;
            ViewCoords = location;
            Action = CursorActions.Click;
            UpdateAction?.Invoke(this);
        }

        public void Down(CursorButtons buttons, MyPoint location)
        {
            if (Parent.Model is null)
                return;

            Buttons = buttons;
            ViewCoords = location;
            Action = CursorActions.Down;
            UpdateAction?.Invoke(this);
        }

        public void Up(CursorButtons buttons, MyPoint location)
        {
            if (Parent.Model is null)
                return;

            Buttons = buttons;
            ViewCoords = location;
            Action = CursorActions.Up;
            UpdateAction?.Invoke(this);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(ViewCoords):
                    WorldCoords = GetWorldCoords(ViewCoords);
                    WorldIndexCoords = Layout.GetIndexPoint(WorldCoords);

                    Info = $"{WorldCoords.X}, {WorldCoords.Y} ({WorldIndexCoords.X}, {WorldIndexCoords.Y})";
                    break;

                case nameof(WorldCoords):
                    Visible = Layout.IndexBounds.Contains(WorldIndexCoords);
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private MyPoint GetWorldCoords(MyPoint point)
        {
            var cellSize = Layout.CellSize;
            var worldCoords = ToWorldCoordsFunc(point);
            return worldCoords;
        }

        private MyPoint GetWorldSnapCoords(MyPoint point)
        {
            var cellSize = Layout.CellSize;
            var worldCoords = ToWorldCoordsFunc(point);

            return new MyPoint((worldCoords.X / cellSize) * cellSize, (worldCoords.Y / cellSize) * cellSize);
        }

        #endregion Private Methods
    }
}