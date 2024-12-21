using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core.Interface;
using OpenBreed.Core.Interface.Extensions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenBreed.Gui.Abstractions.Helpers;
using OpenBreed.Gui.Builders;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Elements
{
    internal class DockPanel : Container, IPanel
    {
        #region Private Fields

        private Box2 dockableBox;

        #endregion Private Fields

        #region Internal Constructors

        internal DockPanel(DockPanelBuilder builder) : base(builder)
        {

            RecalculateChilds();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected void RecalculateChilds()
        {
            dockableBox = new Box2(-Size.X / 2.0f, -Size.Y / 2.0f, Size.X / 2.0f, Size.Y / 2.0f).Deflate(Padding);

            foreach (var child in Childs)
            {
                RecalculateChild(child);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        protected override void Recalculate()
        {
            RecalculateChilds();

            base.Recalculate();
        }

        private void RecalculateChild(IElement child)
        {
            var childNewSize = child.Size.AsVector();

            switch (child.GetDockMode())
            {
                case ElementDockMode.None:
                    break;

                case ElementDockMode.Top:

                    var hBoxesH = dockableBox.SplitHorizontally(Math.Min(child.MaximumSize.Y, dockableBox.HalfSize.Y));

                    child.Position.X = hBoxesH.Top.Center.X;
                    child.Position.Y = hBoxesH.Top.Center.Y;
                    childNewSize.X = hBoxesH.Top.Size.X;
                    childNewSize.Y = hBoxesH.Top.Size.Y;

                    dockableBox = hBoxesH.Bottom;

                    break;

                case ElementDockMode.Bottom:

                    hBoxesH = dockableBox.SplitHorizontally(Math.Min(child.MaximumSize.Y, dockableBox.HalfSize.Y));

                    child.Position.X = hBoxesH.Bottom.Center.X;
                    child.Position.Y = hBoxesH.Bottom.Center.Y;
                    childNewSize.X = hBoxesH.Bottom.Size.X;
                    childNewSize.Y = hBoxesH.Bottom.Size.Y;

                    dockableBox = hBoxesH.Top;

                    break;

                case ElementDockMode.Left:

                    var vBoxes = dockableBox.SplitVertically(Math.Min(child.MaximumSize.X, dockableBox.HalfSize.X));

                    child.Position.X = vBoxes.Left.Center.X;
                    child.Position.Y = vBoxes.Left.Center.Y;
                    childNewSize.X = vBoxes.Left.Size.X;
                    childNewSize.Y = vBoxes.Left.Size.Y;

                    dockableBox = vBoxes.Right;

                    break;

                case ElementDockMode.Right:

                    vBoxes = dockableBox.SplitVertically(Math.Min(child.MaximumSize.X, dockableBox.HalfSize.X));

                    child.Position.X = vBoxes.Right.Center.X;
                    child.Position.Y = vBoxes.Right.Center.Y;
                    childNewSize.X = vBoxes.Right.Size.X;
                    childNewSize.Y = vBoxes.Right.Size.Y;

                    dockableBox = vBoxes.Left;

                    break;

                case ElementDockMode.Fill:

                    child.Position.X = dockableBox.Center.X;
                    child.Position.Y = dockableBox.Center.Y;

                    childNewSize.X = dockableBox.Size.X;
                    childNewSize.Y = dockableBox.Size.Y;

                    break;

                default:
                    break;
            }


            var limitedSize = MyMathHelper.Clamp(childNewSize, child.MinimumSize, child.MaximumSize);
            child.Resize(limitedSize);
        }

        #endregion Private Methods
    }
}