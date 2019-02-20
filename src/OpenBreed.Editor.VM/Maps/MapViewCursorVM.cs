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

        #region Private Fields
        private CursorActions _action;
        private CursorButtons _buttons;
        private Point _viewCoords;
        private bool _visible;
        private Point _worldCoords;
        private Point _worldIndexCoords;

        #endregion Private Fields

        #region Public Constructors

        public MapViewCursorVM()
        {
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<MapViewCursorVM> UpdateAction;

        public Func<Point, Point> CalculateWorldCoordsFunc { get; set; }

        public Func<Point, Point> CalculateWorldIndexCoordsFunc { get; set; }

        public CursorActions Action
        {
            get { return _action; }
            private set { SetProperty(ref _action, value); }
        }

        public CursorButtons Buttons
        {
            get { return _buttons; }
            private set { SetProperty(ref _buttons, value); }
        }

        public Point ViewCoords
        {
            get { return _viewCoords; }
            private set { SetProperty(ref _viewCoords, value); }
        }

        public bool Visible
        {
            get { return _visible; }
            private set { SetProperty(ref _visible, value); }
        }

        public Point WorldCoords
        {
            get { return _worldCoords; }
            set { SetProperty(ref _worldCoords, value); }
        }

        public Point WorldIndexCoords
        {
            get { return _worldIndexCoords; }
            set { SetProperty(ref _worldIndexCoords, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Hover()
        {
            Visible = true;
            Action = CursorActions.Hover;
            UpdateAction?.Invoke(this);
        }

        public void Leave()
        {
            Visible = false;
            Action = CursorActions.Leave;
            UpdateAction?.Invoke(this);
        }

        public void Move(CursorButtons buttons, Point location)
        {
            Buttons = buttons;
            ViewCoords = location;
            Action = CursorActions.Move;
            UpdateAction?.Invoke(this);
        }

        public void Click(CursorButtons buttons, Point location)
        {
            Buttons = buttons;
            ViewCoords = location;
            Action = CursorActions.Click;
            UpdateAction?.Invoke(this);
        }

        public void Down(CursorButtons buttons, Point location)
        {
            Buttons = buttons;
            ViewCoords = location;
            Action = CursorActions.Down;
            UpdateAction?.Invoke(this);
        }

        public void Up(CursorButtons buttons, Point location)
        {
            Buttons = buttons;
            ViewCoords = location;
            Action = CursorActions.Up;
            UpdateAction?.Invoke(this);
        }

        #endregion Public Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewCoords):
                    WorldCoords = CalculateWorldCoordsFunc(ViewCoords);
                    WorldIndexCoords = CalculateWorldIndexCoordsFunc(WorldCoords);
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}
