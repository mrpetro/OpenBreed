using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Levels.Layers
{
    public abstract class MapBodyBaseLayerVM : BaseViewModel
    {
        #region Public Fields

        public readonly Size Size;

        #endregion Public Fields

        #region Private Fields

        private bool _isVisible;
        private string _name;

        #endregion Private Fields

        #region Internal Constructors

        internal MapBodyBaseLayerVM(LevelBodyVM body)
        {
            Body = body;
            Size = body.Size;
            _isVisible = true;
        }

        #endregion Internal Constructors

        #region Public Properties

        public LevelBodyVM Body { get; private set; }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        #endregion Public Properties

        #region Public Methods


        public abstract void Restore(IMapBodyLayerModel layerModel);
        public abstract void DrawView(Graphics gfx, Rectangle rectangle);

        #endregion Public Methods


    }
}
