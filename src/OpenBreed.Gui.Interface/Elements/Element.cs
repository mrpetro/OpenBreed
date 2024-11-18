using OpenBreed.Gui.Interface.Bodies;
using OpenBreed.Gui.Interface.Builders;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core.Interface;
using System.Security.Cryptography;
using OpenBreed.Core.Interface.Extensions;

namespace OpenBreed.Gui.Interface.Elements
{
    internal abstract class Element : IElement
    {
        #region Protected Fields

        protected readonly Action<IElement> clickCallback;
        protected readonly Action<IElement> enterCallback;
        protected readonly Action<IElement> leaveCallback;
        protected readonly Action<IElement> moveCallback;
        protected readonly Action<IElement> downCallback;
        protected readonly Action<IElement> upCallback;
        protected readonly Action<IElement> wheelCallback;

        #endregion Protected Fields

        #region Private Fields

        private readonly List<Element> childs = new List<Element>();
        private Element? parent;
        private Vector2? moveStartPosition;

        private Box2 dockableBox;

        #endregion Private Fields

        #region Internal Constructors

        internal Element(ElementBuilder builder)
        {
            Padding = builder.Padding;
            Margin = builder.Margin;

            Tag = builder.Tag;
            clickCallback = builder.ClickCallback;
            enterCallback = builder.EnterCallback;
            leaveCallback = builder.LeaveCallback;
            moveCallback = builder.MoveCallback;
            downCallback = builder.DownCallback;
            upCallback = builder.UpCallback;
            wheelCallback = builder.WheelCallback;

            Position = new ElementPosition(builder.CenterX, builder.CenterY);
            Body = new ElementBody(builder.Width, builder.Height);
            dockableBox = new Box2(-builder.Width / 2.0f, -builder.Height / 2.0f, builder.Width / 2.0f, builder.Height / 2.0f).Deflate(Padding);

            DockMode = builder.DockMode;

            IsHitTestable = builder.IsHitTestable;
            IsMovable = builder.IsMovable;

            foreach (var child in builder.childBuilders.Select(builder => builder.InternalBuild()))
            {
                child.SetParent(this);
            }
        }

        #endregion Internal Constructors

        #region Public Properties

        public IElementPosition Position { get; set; }

        public IElementBody Body { get; }

        public ElementDockMode DockMode { get; set; }

        public Box2 Padding { get; set; }

        public Box2 Margin { get; set; }

        public string Tag { get; set; }

        public bool IsHitTestable { get; set; }

        public bool IsMovable { get; set; }

        public IElement? Parent => parent;
        public IReadOnlyList<IElement> Childs => childs;

        public bool IsHovered { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public virtual bool HitTest(Vector2 point, out IElement? interactiveElement)
        {
            if (!IsHitTestable)
            {
                interactiveElement = null;
                return false;
            }

            var localPoint = point - Position.AsVector();

            if (!Body.Contains(localPoint.X, localPoint.Y))
            {
                interactiveElement = null;
                return false;
            }

            IElement? childElement = null;

            for (int i = 0; i < childs.Count; i++)
            {
                if (childs[i].HitTest(localPoint, out childElement))
                {
                    break;
                }
            }

            if (childElement is not null)
            {
                interactiveElement = childElement;
                return true;
            }

            interactiveElement = this;
            return true;
        }

        public virtual void OnClick(int cursorId, CursorKey cursorKey)
        {
            if (cursorKey == CursorKey.Left)
            {
                clickCallback?.Invoke(this);
            }
        }

        public virtual void OnEnter(int cursorId)
        {
            IsHovered = true;
            enterCallback?.Invoke(this);
        }

        public virtual void OnLeave(int cursorId)
        {
            leaveCallback?.Invoke(this);
            IsHovered = false;
        }

        public virtual void OnMove(int cursorId, Vector2 position)
        {
            if (IsMovable && moveStartPosition is not null)
            {
                var da = position - moveStartPosition.Value;

                Position.X += da.X;
                Position.Y += da.Y;

                moveStartPosition = position;
            }

            moveCallback?.Invoke(this);
        }

        public virtual void OnDown(int cursorId, Vector2 position, CursorKey cursorKey)
        {
            if (IsMovable)
            {
                moveStartPosition = position;
            }

            downCallback?.Invoke(this);
        }

        public virtual void OnUp(int cursorId, Vector2 position, CursorKey cursorKey)
        {
            if (IsMovable)
            {
                moveStartPosition = null;
            }

            upCallback?.Invoke(this);
        }

        public virtual void OnWheel(int cursorId, int delta)
        {
            wheelCallback?.Invoke(this);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void SetParent(Element newParent)
        {
            if (parent != null && newParent == parent)
            {
                return;
            }

            if (parent != null)
            {
                if (!parent.RemoveChild(this))
                {
                    throw new InvalidOperationException("Expected existing child in parent.");
                }
            }

            parent = newParent;

            parent.AddChild(this);

            RecalculatePositionAndSize();
        }

        #endregion Internal Methods

        #region Protected Methods

        protected void RecalculatePositionAndSize()
        {
            if (parent is null)
            {
                return;
            }

            var box = parent.dockableBox;

            switch (DockMode)
            {
                case ElementDockMode.None:
                    break;

                case ElementDockMode.Top:

                    Position.X = box.Center.X;
                    Position.Y = box.Center.Y + box.Size.Y * 0.25f;
                    Body.Width = box.Size.X;
                    Body.Height = box.Size.Y * 0.5f;

                    parent.dockableBox = Box2Helper.NewBox(
                        box.Center.X,
                        box.Center.Y - box.Size.Y * 0.25f,
                        box.Size.X,
                        box.Size.Y * 0.5f);

                    break;

                case ElementDockMode.Bottom:

                    Position.X = box.Center.X;
                    Position.Y = box.Center.Y - box.Size.Y * 0.25f;
                    Body.Width = box.Size.X;
                    Body.Height = box.Size.Y * 0.5f;

                    parent.dockableBox = Box2Helper.NewBox(
                        box.Center.X,
                        box.Center.Y + box.Size.Y * 0.25f,
                        box.Size.X,
                        box.Size.Y * 0.5f);

                    break;

                case ElementDockMode.Left:

                    Position.X = box.Center.X - box.Size.X * 0.25f;
                    Position.Y = box.Center.Y;
                    Body.Width = box.Size.X * 0.5f;
                    Body.Height = box.Size.Y;

                    parent.dockableBox = Box2Helper.NewBox(
                        box.Center.X + box.Size.X * 0.25f,
                        box.Center.Y,
                        box.Size.X * 0.5f,
                        box.Size.Y);

                    break;

                case ElementDockMode.Right:

                    Position.X = box.Center.X + box.Size.X * 0.25f;
                    Position.Y = box.Center.Y;
                    Body.Width = box.Size.X * 0.5f;
                    Body.Height = box.Size.Y;

                    parent.dockableBox = Box2Helper.NewBox(
                        box.Center.X - box.Size.X * 0.25f,
                        box.Center.Y,
                        box.Size.X * 0.5f,
                        box.Size.Y);

                    break;

                case ElementDockMode.Fill:

                    Position.X = box.Center.X;
                    Position.Y = box.Center.Y;
                    Body.Width = box.Size.X;
                    Body.Height = box.Size.Y;

                    break;

                default:
                    break;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddChild(Element child)
        {
            childs.Add(child);
        }

        private bool RemoveChild(Element child)
        {
            return childs.Remove(child);
        }

        #endregion Private Methods
    }
}