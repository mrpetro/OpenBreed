using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Bodies
{
    internal class ElementBody : IElementBody
    {
        #region Public Constructors

        public ElementBody(float width, float height)
        {
            Width = width;
            Height = height;
        }

        #endregion Public Constructors

        #region Public Properties

        public float Width { get; set; }
        public float Height { get; set; }

        #endregion Public Properties

        #region Public Methods

        public bool Contains(float x, float y)
        {
            if (x < -Width / 2.0f || x > Width / 2.0f)
            {
                return false;
            }

            if (y < -Height / 2.0f || y > Height / 2.0f)
            {
                return false;
            }

            return true;
        }

        #endregion Public Methods
    }
}