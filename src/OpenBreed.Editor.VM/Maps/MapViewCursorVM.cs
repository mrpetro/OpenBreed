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
    public class MapViewCursorVM : BaseViewModel
    {

        #region Private Fields

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

        public Func<Point, Point> CalculateWorldCoordsFunc { get; set; }

        public Func<Point, Point> CalculateWorldIndexCoordsFunc { get; set; }

        public Point ViewCoords
        {
            get { return _viewCoords; }
            set { SetProperty(ref _viewCoords, value); }
        }

        public bool Visible
        {
            get { return _visible; }
            set { SetProperty(ref _visible, value); }
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
