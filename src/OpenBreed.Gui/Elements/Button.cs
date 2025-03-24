using System;
using System.Collections.Generic;
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
    internal class Button : Element, IButton
    {
        #region Internal Constructors

        internal Button(ButtonBuilder builder) : base(builder)
        {
        }

        #endregion Internal Constructors

        #region Public Properties

        public bool IsPressed { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public override void OnCursorClick(IInteractionCursor cursor, CursorKey cursorKey)
        {
            base.OnCursorClick(cursor, cursorKey);
        }

        public override void OnCursorDown(IInteractionCursor cursor, CursorKey cursorKey)
        {
            IsPressed = true;
            base.OnCursorDown(cursor, cursorKey);
        }

        public override void OnCursorUp(IInteractionCursor cursor, CursorKey cursorKey)
        {
            IsPressed = false;
            base.OnCursorUp(cursor, cursorKey);
        }

        #endregion Public Methods
    }
}