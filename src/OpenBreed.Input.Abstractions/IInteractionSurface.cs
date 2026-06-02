using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Input.Abstractions
{
    internal interface IInteractionSurface
    {
        #region Public Properties

        Guid Id { get; }

        #endregion Public Properties

        #region Public Methods

        Vector4 FromHostToWorldPoint(Vector2i point);

        #endregion Public Methods
    }
}