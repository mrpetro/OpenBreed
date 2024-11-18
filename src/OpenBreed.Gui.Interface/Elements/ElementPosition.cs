using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Elements
{
    public class ElementPosition : IElementPosition
    {
        #region Public Constructors

        public ElementPosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        #endregion Public Constructors

        #region Public Properties

        public float X { get; set; }
        public float Y { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Vector2 AsVector() => new Vector2(X, Y);

        #endregion Public Methods
    }
}