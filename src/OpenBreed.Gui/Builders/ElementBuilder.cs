using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Controllers;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Abstractions.Events;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenBreed.Gui.Builders
{
    internal abstract class ElementBuilder<TElement> : ElementBuilder, IElementBuilder<TElement> where TElement : IElement
    {
        protected ElementBuilder(IElementInputController inputController) : base(inputController)
        {
        }

        #region Public Methods

        public abstract TElement Build();

        #endregion Public Methods
    }

    internal abstract class ElementBuilder : IElementBuilder
    {
        #region Public Constructors

        public ElementBuilder(IElementInputController inputController)
        {
            InputController = inputController;
        }

        #endregion Public Constructors

        #region Internal Properties

        internal IElementInputController InputController { get; }
        internal List<IElementOption> Options { get; } = new List<IElementOption>();

        internal PositionSystem PositionSystem { get; private set; }
        internal Action<IElement, IInteractionCursor, CursorKey> ClickCallback { get; private set; }
        internal Action<IElement, Vector2> MoveCallback { get; private set; }
        internal string Tag { get; private set; }
        internal Vector2 Size { get; private set; }
        internal Vector2 MinimumSize { get; private set; } = Vector2.One;
        internal Vector2 MaximumSize { get; private set; } = new Vector2(float.MaxValue, float.MaxValue);
        internal Vector2 Position { get; private set; }
        internal bool IsHitTestable { get; private set; } = true;
        internal bool IsMovable { get; private set; } = false;
        internal Box2 Padding { get; private set; }
        internal Box2 Margin { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public void SetClickCallback(Action<IElement, IInteractionCursor, CursorKey> callback)
        {
            ClickCallback = callback;
        }

        public void SetMoveCallback(Action<IElement, Vector2> callback)
        {
            MoveCallback = callback;
        }

        public void SetPosition(float x, float y, PositionSystem positionSystem = PositionSystem.Parent)
        {
            Position = new Vector2(x, y);
            PositionSystem = positionSystem;
        }

        public void SetSize(float width, float height)
        {
            Size = new Vector2(width, height);
        }

        public void SetMaximumSize(float width, float height)
        {
            MaximumSize = new Vector2(width, height);
        }

        public void SetMinimumSize(float width, float height)
        {
            MinimumSize = new Vector2(width, height);
        }

        public void SetTag(string tag)
        {
            Tag = tag;
        }

        public void SetPadding(float padding) => SetPadding(padding, padding, padding, padding);

        public void SetPadding(float left, float bottom, float right, float top)
        {
            left = Math.Clamp(left, 0, float.MaxValue);
            bottom = Math.Clamp(bottom, 0, float.MaxValue);
            right = Math.Clamp(right, 0, float.MaxValue);
            top = Math.Clamp(top, 0, float.MaxValue);

            Padding = new Box2(left, bottom, right, top);
        }

        public void SetMargin(float margin) => SetMargin(margin, margin, margin, margin);

        public void SetMargin(float left, float bottom, float right, float top)
        {
            left = Math.Clamp(left, 0, float.MaxValue);
            bottom = Math.Clamp(bottom, 0, float.MaxValue);
            right = Math.Clamp(right, 0, float.MaxValue);
            top = Math.Clamp(top, 0, float.MaxValue);

            Margin = new Box2(left, bottom, right, top);
        }

        public void SetHitTestable(bool flag)
        {
            IsHitTestable = flag;
        }

        public void SetMovable(bool flag)
        {
            IsMovable = flag;
        }

        public void SetOption(IElementOption parentOption)
        {
            Options.Add(parentOption);
        }

        #endregion Public Methods
    }
}