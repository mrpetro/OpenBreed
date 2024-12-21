using OpenBreed.Gui.Abstractions.Elements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Elements
{
    public class ElementPosition : IElementPosition
    {
        #region Public Constructors

        public ElementPosition(Vector2 position)
        {
            X = position.X;
            Y = position.Y;
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