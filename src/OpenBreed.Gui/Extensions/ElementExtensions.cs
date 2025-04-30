using OpenBreed.Gui.Abstractions.Elements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Extensions
{
    public static class ElementExtensions
    {
        #region Public Methods

        public static Vector2 GetLocalPosition(this IElement element, Vector2 position)
        {
            var reverseCoords = element.Position.AsVector();

            while (element.Parent is not null)
            {
                element = element.Parent;

                reverseCoords += element.Position.AsVector();
            }

            var localPosition = position - reverseCoords;

            return localPosition;
        }

        #endregion Public Methods
    }
}