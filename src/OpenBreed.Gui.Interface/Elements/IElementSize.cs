using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Elements
{
    public interface IElementSize
    {
        #region Public Properties

        float X { get; set; }
        float Y { get; set; }

        #endregion Public Properties

        #region Public Methods

        Vector2 AsVector();

        #endregion Public Methods
    }
}