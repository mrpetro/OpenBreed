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

    internal class Statebox : Element, IStatebox
    {
        #region Private Fields

        private readonly PropertyBinding<bool> isCheckedBinding;

        #endregion Private Fields

        #region Internal Constructors

        internal Statebox(StateboxBuilder builder) : base(builder)
        {
            IsChecked = builder.IsChecked;

            isCheckedBinding = builder.IsCheckedBinding;

            if (isCheckedBinding is not null)
            {
                IsChecked = isCheckedBinding.GetValue();
            }
        }

        #endregion Internal Constructors

        #region Public Properties

        public bool IsPressed { get; private set; }

        public bool IsChecked { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public override void OnClick(IInteractionCursor cursor, CursorKey cursorKey)
        {
            IsChecked = !IsChecked;

            if (isCheckedBinding is not null)
            {
                isCheckedBinding.SetValue(IsChecked);
            }

            base.OnClick(cursor, cursorKey);
        }

        public override void OnDown(IInteractionCursor cursor, CursorKey cursorKey)
        {
            IsPressed = true;
            base.OnDown(cursor, cursorKey);
        }

        public override void OnUp(IInteractionCursor cursor, CursorKey cursorKey)
        {
            IsPressed = false;
            base.OnUp(cursor, cursorKey);
        }

        #endregion Public Methods
    }
}