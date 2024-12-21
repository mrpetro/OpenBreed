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

        private Container? parent;
        private Vector2? moveStartPosition;

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
            Size = new ElementSize(builder.Size);

            Options = builder.Options;

            IsHitTestable = builder.IsHitTestable;
            IsMovable = builder.IsMovable;
        }

        #endregion Protected Constructors

        #region Public Properties

        public IList<IElementOption> Options { get; }
        public IElementPosition Position { get; set; }

        public IElementSize Size { get; set; }

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

        public void Resize(Vector2 newSize, ElementResizeAnchor anchor)
        {
            Size.X = newSize.X;
            Size.Y = newSize.Y;

            Recalculate();
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

        protected virtual void Recalculate()
        {
        }

        #endregion Protected Methods
    }
}