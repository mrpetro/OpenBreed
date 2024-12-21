using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using OpenBreed.Core.Interface.Extensions;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenBreed.Gui.Abstractions.Helpers;
using System.Drawing;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Builders;

namespace OpenBreed.Gui.Elements
{
    internal abstract class Element : IElement
    {
        #region Protected Fields

        protected readonly Action<IElement> clickCallback;
        protected readonly Action<IElement> enterCallback;
        protected readonly Action<IElement> leaveCallback;
        protected readonly Action<IElement, Vector2> moveCallback;
        protected readonly Action<IElement> downCallback;
        protected readonly Action<IElement> upCallback;
        protected readonly Action<IElement> wheelCallback;

        #endregion Protected Fields

        #region Private Fields

        private Container? parent;

        #endregion Private Fields

        #region Protected Constructors

        protected Element(ElementBuilder builder)
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

            Position = new ElementPosition(builder.Position);

            MinimumSize = builder.MinimumSize;
            MaximumSize = builder.MaximumSize;

            Size = new ElementSize(MyMathHelper.Clamp(builder.Size, MinimumSize, MaximumSize));

            Options = builder.Options;

            IsHitTestable = builder.IsHitTestable;
            IsMovable = builder.IsMovable;
        }

        #endregion Protected Constructors

        #region Public Properties

        public IList<IElementOption> Options { get; }

        public IElementPosition Position { get; set; }

        public IElementSize Size { get; set; }

        public Vector2 MinimumSize { get; }

        public Vector2 MaximumSize { get; }

        public Box2 LocalBox
        {
            get
            {
                return Box2Helper.NewBox(Vector2.Zero, Size.AsVector()).Deflate(Margin);
            }
        }

        public Box2 ActualBox
        {
            get
            {
                return LocalBox.Translated(Position.AsVector());
            }
        }

        public Box2 Padding { get; set; }

        public Box2 Margin { get; set; }

        public string Tag { get; set; }

        public bool IsHitTestable { get; set; }

        public bool IsMovable { get; set; }

        public IElement? Parent => parent;

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

            if (!LocalBox.ContainsInclusive(localPoint))
            {
                interactiveElement = null;
                return false;
            }

            interactiveElement = this;
            return true;
        }

        public virtual void OnClick(IInteractionCursor cursor, CursorKey cursorKey)
        {
            if (cursorKey == CursorKey.Left)
            {
                clickCallback?.Invoke(this);
            }
        }

        public virtual void OnEnter(IInteractionCursor cursor)
        {
            IsHovered = true;
            enterCallback?.Invoke(this);
        }

        public virtual void OnLeave(IInteractionCursor cursor)
        {
            leaveCallback?.Invoke(this);
            IsHovered = false;
        }

        public void MoveBy(Vector2 offset)
        {
            Position.X += offset.X;
            Position.Y += offset.Y;

            OnMove(offset);
        }

        public virtual void OnCursorMove(IInteractionCursor cursor)
        {
        }

        public virtual void OnDown(IInteractionCursor cursor, CursorKey cursorKey)
        {
            downCallback?.Invoke(this);
        }

        public virtual void OnUp(IInteractionCursor cursor, CursorKey cursorKey)
        {
            upCallback?.Invoke(this);
        }

        public virtual void OnWheel(IInteractionCursor cursor)
        {
            wheelCallback?.Invoke(this);
        }

        public IElement? GetAncestor(string tag)
        {
            if (tag is null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            var ancestor = parent;

            while (ancestor is not null)
            {
                if (ancestor.Tag == tag)
                {
                    return ancestor;
                }

                ancestor = ancestor.parent;
            }

            return ancestor;
        }

        public void Resize(Vector2 newSize)
        {
            Size.X = newSize.X;
            Size.Y = newSize.Y;

            Recalculate();
        }

        public void ResizeBy(Vector2 offset, ElementResizeAnchor anchor)
        {
            var newSize = Size.AsVector();

            switch (anchor)
            {
                case ElementResizeAnchor.Center:

                    newSize.X += offset.X * 2.0f;
                    newSize.Y -= offset.Y * 2.0f;

                    break;

                case ElementResizeAnchor.Left:

                    Position.X += offset.X * 0.5f;
                    newSize.X -= offset.X;

                    break;

                case ElementResizeAnchor.LeftTop:

                    Position.X += offset.X * 0.5f;
                    Position.Y += offset.Y * 0.5f;

                    newSize.X -= offset.X;
                    newSize.Y += offset.Y;

                    break;

                case ElementResizeAnchor.Top:

                    Position.Y += offset.Y * 0.5f;
                    newSize.Y += offset.Y;

                    break;

                case ElementResizeAnchor.TopRight:

                    Position.X += offset.X * 0.5f;
                    Position.Y += offset.Y * 0.5f;

                    newSize.X += offset.X;
                    newSize.Y += offset.Y;

                    break;

                case ElementResizeAnchor.Right:

                    Position.X += offset.X * 0.5f;
                    newSize.X += offset.X;

                    break;

                case ElementResizeAnchor.RightBottom:

                    Position.X += offset.X * 0.5f;
                    Position.Y += offset.Y * 0.5f;

                    newSize.X += offset.X;
                    newSize.Y -= offset.Y;

                    break;

                case ElementResizeAnchor.Bottom:

                    Position.Y += offset.Y * 0.5f;
                    newSize.Y -= offset.Y;

                    break;

                case ElementResizeAnchor.BottomLeft:

                    Position.X += offset.X * 0.5f;
                    Position.Y += offset.Y * 0.5f;

                    newSize.X -= offset.X;
                    newSize.Y -= offset.Y;

                    break;

                default:
                    break;
            }

            var limitedSize = MyMathHelper.Clamp(newSize, MinimumSize, MaximumSize);

            switch (anchor)
            {
                case ElementResizeAnchor.Center:

                    newSize.X = limitedSize.X;
                    newSize.Y = limitedSize.Y;

                    break;
                default:

                    var sizeDelta = limitedSize - newSize;

                    Position.X -= Math.Sign(offset.X) * Math.Abs(sizeDelta.X) * 0.5f;
                    Position.Y -= Math.Sign(offset.Y) * Math.Abs(sizeDelta.Y) * 0.5f;

                    newSize.X = limitedSize.X;
                    newSize.Y = limitedSize.Y;
                    break;
            }

            Resize(newSize);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void SetParent(Container newParent)
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
        }

        #endregion Internal Methods

        #region Protected Methods

        protected virtual void OnMove(Vector2 offset)
        {
            moveCallback?.Invoke(this, offset);
        }

        protected virtual void Recalculate()
        {
        }

        #endregion Protected Methods
    }
}