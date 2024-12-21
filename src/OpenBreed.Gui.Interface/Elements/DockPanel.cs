using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core.Interface;
using OpenBreed.Core.Interface.Extensions;
using OpenBreed.Gui.Interface.Builders;
using OpenBreed.Gui.Interface.Extensions;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Interface.Elements
{
    internal class DockPanel : Container, IPanel
    {
        #region Private Fields

        private Box2 dockableBox;

        #endregion Private Fields

        #region Internal Constructors

        internal DockPanel(DockPanelBuilder builder) : base(builder)
        {
            dockableBox = new Box2(-Size.X / 2.0f, -Size.Y / 2.0f, Size.X / 2.0f, Size.Y / 2.0f).Deflate(Padding);

            RecalculateChilds();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected void RecalculateChilds()
        {
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
            var box = dockableBox;

            switch (child.GetDockMode())
            {
                case ElementDockMode.None:
                    break;

                case ElementDockMode.Top:

                    child.Position.X = box.Center.X;
                    child.Position.Y = box.Center.Y + box.Size.Y * 0.25f;

                    child.Size.X = box.Size.X;
                    child.Size.Y = box.Size.Y * 0.5f;

                    dockableBox = Box2Helper.NewBox(
                        box.Center.X,
                        box.Center.Y - box.Size.Y * 0.25f,
                        box.Size.X,
                        box.Size.Y * 0.5f);

                    break;

                case ElementDockMode.Bottom:

                    child.Position.X = box.Center.X;
                    child.Position.Y = box.Center.Y - box.Size.Y * 0.25f;

                    child.Size.X = box.Size.X;
                    child.Size.Y = box.Size.Y * 0.5f;

                    dockableBox = Box2Helper.NewBox(
                        box.Center.X,
                        box.Center.Y + box.Size.Y * 0.25f,
                        box.Size.X,
                        box.Size.Y * 0.5f);

                    break;

                case ElementDockMode.Left:

                    child.Position.X = box.Center.X - box.Size.X * 0.25f;
                    child.Position.Y = box.Center.Y;

                    child.Size.X = box.Size.X * 0.5f;
                    child.Size.Y = box.Size.Y;

                    dockableBox = Box2Helper.NewBox(
                        box.Center.X + box.Size.X * 0.25f,
                        box.Center.Y,
                        box.Size.X * 0.5f,
                        box.Size.Y);

                    break;

                case ElementDockMode.Right:

                    child.Position.X = box.Center.X + box.Size.X * 0.25f;
                    child.Position.Y = box.Center.Y;

                    child.Size.X = box.Size.X * 0.5f;
                    child.Size.Y = box.Size.Y;

                    dockableBox = Box2Helper.NewBox(
                        box.Center.X - box.Size.X * 0.25f,
                        box.Center.Y,
                        box.Size.X * 0.5f,
                        box.Size.Y);

                    break;

                case ElementDockMode.Fill:

                    child.Position.X = box.Center.X;
                    child.Position.Y = box.Center.Y;

                    child.Size.X = box.Size.X;
                    child.Size.Y = box.Size.Y;

                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}