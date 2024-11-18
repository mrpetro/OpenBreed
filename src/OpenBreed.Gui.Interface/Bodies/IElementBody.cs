using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Bodies
{
    public interface IElementBody
    {
        #region Public Properties

        public float Width { get; set; }
        public float Height { get; set; }

        #endregion Public Properties

        #region Public Methods

        bool Contains(float x, float y);

        #endregion Public Methods
    }
}