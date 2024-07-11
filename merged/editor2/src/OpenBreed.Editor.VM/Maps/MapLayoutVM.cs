using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapLayoutVM : BaseViewModel
    {
        #region Private Fields

        private Size _size;

        #endregion Private Fields

        #region Public Constructors

        public MapLayoutVM(MapEditorVM parent)
        {
            Parent = parent;
        }

        #endregion Public Constructors

        #region Public Properties

        public float MaxCoordX { get; private set; }
        public float MaxCoordY { get; private set; }
        public MapEditorVM Parent { get; }

        public Size Size
        {
            get { return _size; }
            set { SetProperty(ref _size, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        #endregion Internal Methods
    }
}