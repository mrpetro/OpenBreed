using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Interface.Elements;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Interface.Builders
{
    internal abstract class ElementBuilder : IElementBuilder
    {
        #region Internal Fields

        internal readonly List<ElementBuilder> childBuilders = new List<ElementBuilder>();

        #endregion Internal Fields

        #region Private Fields

        private readonly ElementBuilder parentBuilder;

        #endregion Private Fields

        #region Public Constructors

        public ElementBuilder(ElementBuilder parentBuilder)
        {
            this.parentBuilder = parentBuilder;
        }

        #endregion Public Constructors

        #region Internal Properties

        internal PositionSystem PositionSystem { get; private set; }
        internal Action<IElement> ClickCallback { get; private set; }
        internal Action<IElement> EnterCallback { get; private set; }
        internal Action<IElement> LeaveCallback { get; private set; }
        internal Action<IElement> MoveCallback { get; private set; }
        internal Action<IElement> DownCallback { get; private set; }
        internal Action<IElement> UpCallback { get; private set; }
        internal Action<IElement> WheelCallback { get; private set; }
        internal ElementDockMode DockMode { get; private set; }
        internal string Tag { get; private set; }
        internal int Width { get; private set; }
        internal int Height { get; private set; }
        internal float CenterX { get; private set; }
        internal float CenterY { get; private set; }
        internal bool IsHitTestable { get; private set; } = true;
        internal bool IsMovable { get; private set; } = false;
        internal Box2 Padding { get; private set; }
        internal Box2 Margin { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public IElement Build()
        {
            if (parentBuilder is not null)
            {
                throw new InvalidOperationException($"Invalid level of calling {nameof(Build)}()");
            }

            var element = InternalBuild();

            return element;
        }

        public IElementBuilder AddLabel(Action<ILabelBuilder> setter)
        {
            var builder = new LabelBuilder(this);

            setter.Invoke(builder);

            return AddChild(builder);
        }

        public IElementBuilder AddPanel(Action<IPanelBuilder> setter)
        {
            var builder = new PanelBuilder(this);

            setter.Invoke(builder);

            return AddChild(builder);
        }

        public IElementBuilder AddButton(Action<IButtonBuilder> setter)
        {
            var builder = new ButtonBuilder(this);

            setter.Invoke(builder);

            return AddChild(builder);
        }

        public IElementBuilder AddCheckbox(Action<ICheckboxBuilder> setter)
        {
            var builder = new CheckboxBuilder(this);

            setter.Invoke(builder);

            return AddChild(builder);
        }

        public void SetClickCallback(Action<IElement> callback)
        {
            ClickCallback = callback;
        }

        public void SetEnterCallback(Action<IElement> callback)
        {
            EnterCallback = callback;
        }

        public void SetLeaveCallback(Action<IElement> callback)
        {
            LeaveCallback = callback;
        }

        public void SetMoveCallback(Action<IElement> callback)
        {
            MoveCallback = callback;
        }

        public void SetDownCallback(Action<IElement> callback)
        {
            DownCallback = callback;
        }

        public void SetUpCallback(Action<IElement> callback)
        {
            UpCallback = callback;
        }

        public void SetWheelCallback(Action<IElement> callback)
        {
            WheelCallback = callback;
        }

        public void SetDock(ElementDockMode dockMode)
        {
            DockMode = dockMode;
        }

        public void SetPosition(float x, float y, PositionSystem positionSystem = PositionSystem.Parent)
        {
            CenterX = x;
            CenterY = y;
            PositionSystem = positionSystem;
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void SetTag(string tag)
        {
            Tag = tag;
        }

        public void SetPadding(float padding) => SetPadding(padding, padding, padding, padding);

        public void SetPadding(float left, float bottom, float right, float top)
        {
            Padding = new Box2(left, bottom, right, top);
        }

        public void SetMargin(float margin) => SetMargin(margin, margin, margin, margin);

        public void SetMargin(float left, float bottom, float right, float top)
        {
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

        public IElementBuilder AddChild(ElementBuilder childBuilder)
        {
            childBuilders.Add(childBuilder);
            return this;
        }

        #endregion Public Methods

        #region Internal Methods

        internal abstract Element InternalBuild();

        #endregion Internal Methods
    }
}