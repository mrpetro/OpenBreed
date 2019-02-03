using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Tools
{
    public interface IZoomableVM
    {
        #region Public Properties

        float ZoomScale { get; }

        #endregion Public Properties

        #region Public Methods

        void ZoomViewTo(Point location, float scale);

        #endregion Public Methods
    }
}
