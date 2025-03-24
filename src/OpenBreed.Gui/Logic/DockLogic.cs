using OpenBreed.Core.Interface.Extensions;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenBreed.Gui.Abstractions.Logic;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Logic
{
    internal class DockLogic : IDockLogic
    {
        #region Public Methods

        public void Apply(IElement child, Box2 hostBox)
        {
            var childNewSize = child.Size.AsVector();

            switch (child.GetDockMode())
            {
                case ElementDockMode.None:
                    break;

                case ElementDockMode.Top:

                    var hBoxesH = hostBox.SplitHorizontally(Math.Min(child.MaximumSize.Y, hostBox.HalfSize.Y));

                    child.Position.X = hBoxesH.Top.Center.X;
                    child.Position.Y = hBoxesH.Top.Center.Y;
                    childNewSize.X = hBoxesH.Top.Size.X;
                    childNewSize.Y = hBoxesH.Top.Size.Y;

                    hostBox = hBoxesH.Bottom;

                    break;

                case ElementDockMode.Bottom:

                    hBoxesH = hostBox.SplitHorizontally(Math.Min(child.MaximumSize.Y, hostBox.HalfSize.Y));

                    child.Position.X = hBoxesH.Bottom.Center.X;
                    child.Position.Y = hBoxesH.Bottom.Center.Y;
                    childNewSize.X = hBoxesH.Bottom.Size.X;
                    childNewSize.Y = hBoxesH.Bottom.Size.Y;

                    hostBox = hBoxesH.Top;

                    break;

                case ElementDockMode.Left:

                    var vBoxes = hostBox.SplitVertically(Math.Min(child.MaximumSize.X, hostBox.HalfSize.X));

                    child.Position.X = vBoxes.Left.Center.X;
                    child.Position.Y = vBoxes.Left.Center.Y;
                    childNewSize.X = vBoxes.Left.Size.X;
                    childNewSize.Y = vBoxes.Left.Size.Y;

                    hostBox = vBoxes.Right;

                    break;

                case ElementDockMode.Right:

                    vBoxes = hostBox.SplitVertically(Math.Min(child.MaximumSize.X, hostBox.HalfSize.X));

                    child.Position.X = vBoxes.Right.Center.X;
                    child.Position.Y = vBoxes.Right.Center.Y;
                    childNewSize.X = vBoxes.Right.Size.X;
                    childNewSize.Y = vBoxes.Right.Size.Y;

                    hostBox = vBoxes.Left;

                    break;

                case ElementDockMode.Fill:

                    child.Position.X = hostBox.Center.X;
                    child.Position.Y = hostBox.Center.Y;

                    childNewSize.X = hostBox.Size.X;
                    childNewSize.Y = hostBox.Size.Y;

                    break;

                default:
                    break;
            }
        }

        #endregion Public Methods
    }
}