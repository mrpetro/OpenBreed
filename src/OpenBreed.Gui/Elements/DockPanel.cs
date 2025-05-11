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
using OpenBreed.Gui.Logic;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Elements
{
    internal class DockPanel : Container, IDockPanel
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

                    var sizeY = Math.Min(child.MaximumSize.Y, dockableBox.HalfSize.Y);

                    var hBoxes = dockableBox.SplitHorizontally(dockableBox.Size.Y - sizeY);

                    childNewSize = MyMathHelper.Clamp(hBoxes.Top.Size, child.MinimumSize, child.MaximumSize);

                    child.Position.X = hBoxes.Top.Center.X;
                    child.Position.Y = hBoxes.Top.Center.Y;

                    dockableBox = hBoxes.Bottom;

                    break;

                case ElementDockMode.Bottom:

                    sizeY = Math.Min(child.MaximumSize.Y, dockableBox.HalfSize.Y);

                    hBoxes = dockableBox.SplitHorizontally(sizeY);

                    childNewSize = MyMathHelper.Clamp(hBoxes.Bottom.Size, child.MinimumSize, child.MaximumSize);

                    child.Position.X = hBoxes.Bottom.Center.X;
                    child.Position.Y = hBoxes.Bottom.Center.Y;

                    dockableBox = hBoxes.Top;

                    break;

                case ElementDockMode.Left:

                    var sizeX = Math.Min(child.MaximumSize.X, dockableBox.HalfSize.X);

                    var vBoxes = dockableBox.SplitVertically(sizeX);

                    childNewSize = MyMathHelper.Clamp(vBoxes.Left.Size, child.MinimumSize, child.MaximumSize);

                    child.Position.X = vBoxes.Left.Center.X;
                    child.Position.Y = vBoxes.Left.Center.Y;

                    dockableBox = vBoxes.Right;

                    break;

                case ElementDockMode.Right:

                    sizeX = Math.Min(child.MaximumSize.X, dockableBox.HalfSize.X);

                    vBoxes = dockableBox.SplitVertically(dockableBox.Size.X - sizeX);

                    childNewSize = MyMathHelper.Clamp(vBoxes.Right.Size, child.MinimumSize, child.MaximumSize);

                    child.Position.X = vBoxes.Right.Center.X;
                    child.Position.Y = vBoxes.Right.Center.Y;

                    dockableBox = vBoxes.Left;

                    break;

                case ElementDockMode.Fill:

                    childNewSize = MyMathHelper.Clamp(dockableBox.Size, child.MinimumSize, child.MaximumSize);

                    child.Position.X = dockableBox.Center.X;
                    child.Position.Y = dockableBox.Center.Y;

                    break;

                default:
                    break;
            }

            child.Resize(childNewSize);
        }

        #endregion Private Methods
    }
}