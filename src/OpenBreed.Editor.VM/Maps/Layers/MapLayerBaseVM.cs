using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps.Layers
{
    public abstract class MapLayerBaseVM : BaseViewModel
    {

        #region Private Fields

        private bool _isVisible;
        private string _name;

        #endregion Private Fields

        #region Internal Constructors

        internal MapLayerBaseVM(MapLayoutVM layout)
        {
            Layout = layout;
            Size = layout.Size;
            _isVisible = true;
        }

        #endregion Internal Constructors

        #region Public Properties

        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        public MapLayoutVM Layout { get; }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public Size Size { get; }

        #endregion Public Properties

        #region Public Methods

        public abstract void Restore(IMapLayerModel layerModel);

        #endregion Public Methods

    }
}
