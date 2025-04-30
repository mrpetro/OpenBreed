using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Builders;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Elements
{
    internal class CheckField : Element, ICheckField
    {
        #region Private Fields

        private readonly PropertyBinding<bool>? valueBinding;

        #endregion Private Fields

        #region Internal Constructors

        internal CheckField(CheckFieldBuilder builder) : base(builder)
        {
            valueBinding = builder.ValueBinding;

            if (valueBinding is not null)
            {
                Value = valueBinding.GetValue();
            }
            else
            {
                Value = builder.Value;
            }
        }

        #endregion Internal Constructors

        #region Public Properties

        public bool IsPressed { get; private set; }

        public bool Value { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public override void Click(IInteractionCursor cursor, CursorKey cursorKey)
        {
            Value = !Value;
            valueBinding?.SetValue(Value);

            base.Click(cursor, cursorKey);
        }

        public void Press()
        {
            IsPressed = true;
        }

        public void Release()
        {
            IsPressed = false;
        }

        #endregion Public Methods
    }
}