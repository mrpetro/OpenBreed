using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Helpers;
using OpenBreed.Gui.Builders;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Elements
{
    internal class Scrollbar : Element, IScrollbar
    {
        #region Private Fields

        private readonly IValueBinding<float> valueBinding;
        private Vector2? grabOnHandlePosition = null;

        #endregion Private Fields

        #region Public Constructors

        public Scrollbar(ScrollbarBuilder builder) : base(builder)
        {
            Mode = builder.Mode;
            ValueUnit = builder.ValueUnit;
            MinimumValue = builder.MinimumValue;
            MaximumValue = builder.MaximumValue;

            valueBinding = builder.ValueBinding;

            if (valueBinding is null)
            {
                valueBinding = ValueBinding<float>.Create(builder.DefaultValue);
            }

            ClampValue(valueBinding.GetValue());
        }

        #endregion Public Constructors

        #region Public Properties

        public float Value
        {
            get => valueBinding.GetValue();
            private set => ClampValue(value);
        }

        public ScrollbarMode Mode { get; }

        public float ValueUnit { get; private set; }

        public float MinimumValue { get; private set; }

        public float MaximumValue { get; private set; }

        public bool IsHandleHovered { get; private set; }

        public bool IsHandleGrabbed { get; private set; }

        public Box2 HandleBox { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void GrabHandle(Vector2 grabOnHandlePosition)
        {
            this.grabOnHandlePosition = grabOnHandlePosition;
            IsHandleGrabbed = true;
        }

        public void ReleaseHandle()
        {
            grabOnHandlePosition = null;
            IsHandleGrabbed = false;
        }

        public void MoveHandle(Vector2 newOnHandlePosition)
        {
            var offset = newOnHandlePosition - grabOnHandlePosition ?? Vector2.Zero;

            var handleMinPos = GetHandleMinimumPosition();
            var handleMaxPos = GetHandleMaximumPosition();
            var handlePos = GetHandlePosition();
            var direction = GetHandleDirection();

            var dotOffset = Vector2.Dot(offset, direction);

            handlePos += dotOffset;

            handlePos = Math.Clamp(handlePos, handleMinPos, handleMaxPos);

            var newPosition = handlePos * direction;

            Value = GetValueFromHandlePosition(handlePos, handleMinPos, handleMaxPos);

            HandleBox = Box2Helper.NewBox(newPosition, HandleBox.Size);
        }

        public void EnterHandle()
        {
            IsHandleHovered = true;
        }

        public void LeaveHandle()
        {
            IsHandleHovered = false;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Recalculate()
        {
            base.Recalculate();

            RefreshHandleBox();
        }

        #endregion Protected Methods

        #region Private Methods

        private void ClampValue(float value)
        {
            var clampedValued = MathHelper.Clamp(value, MinimumValue, MaximumValue);
            valueBinding.SetValue(clampedValued);
        }

        private float GetHandleMinimumPosition() =>
            Mode switch
            {
                ScrollbarMode.Horizontal => LocalBox.Min.X + HandleBox.Size.X / 2.0f,
                ScrollbarMode.Vertical => LocalBox.Min.Y + HandleBox.Size.Y / 2.0f,
                _ => throw new NotImplementedException()
            };

        private float GetHandleMaximumPosition() =>
            Mode switch
            {
                ScrollbarMode.Horizontal => LocalBox.Max.X - HandleBox.Size.X / 2.0f,
                ScrollbarMode.Vertical => LocalBox.Max.Y - HandleBox.Size.Y / 2.0f,
                _ => throw new NotImplementedException()
            };

        private float GetHandlePosition() =>
            Mode switch
            {
                ScrollbarMode.Horizontal => HandleBox.Center.X,
                ScrollbarMode.Vertical => HandleBox.Center.Y,
                _ => throw new NotImplementedException()
            };

        private Vector2 GetHandleDirection() => Mode switch
        {
            ScrollbarMode.Horizontal => Vector2.UnitX,
            ScrollbarMode.Vertical => Vector2.UnitY,
            _ => throw new NotImplementedException()
        };

        private float GetValueFromHandlePosition(float position, float maxPosition, float minPosition)
        {
            var normalizedPosition = (position - minPosition) / (maxPosition - minPosition);

            var value = (MaximumValue - MinimumValue) * normalizedPosition + MinimumValue;

            return value;
        }

        private float GetHandlePositionFromValue()
        {
            var normalizedValue = (Value - MinimumValue) / (MaximumValue - MinimumValue);

            var handleMinPos = GetHandleMinimumPosition();
            var handleMaxPos = GetHandleMaximumPosition();

            var handlePos = (handleMaxPos - handleMinPos) * normalizedValue + handleMinPos;

            return handlePos;
        }

        private void RefreshHandleBox()
        {
            var range = MaximumValue - MinimumValue;
            var sizeX = 0.0f;
            var sizeY = 0.0f;

            switch (Mode)
            {
                case ScrollbarMode.Horizontal:
                    sizeX = LocalBox.Size.X / range * ValueUnit;
                    sizeY = LocalBox.Size.Y;
                    break;

                case ScrollbarMode.Vertical:
                    sizeX = LocalBox.Size.X;
                    sizeY = LocalBox.Size.Y / range * ValueUnit;
                    break;

                default:
                    break;
            }

            var handleBox = new Box2(-sizeX / 2.0f, -sizeY / 2.0f, sizeX / 2.0f, sizeY / 2.0f);

            HandleBox = handleBox;

            var handlePos = GetHandlePositionFromValue();
            var handleDir = GetHandleDirection();

            handleBox.Translate(handleDir * handlePos);

            HandleBox = handleBox;
        }

        #endregion Private Methods
    }
}