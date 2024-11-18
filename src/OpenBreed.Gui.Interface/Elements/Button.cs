using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Interface.Builders;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Interface.Elements
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

        public override void OnClick(int cursorId, CursorKey cursorKey)
        {
            base.OnClick(cursorId, cursorKey);
        }

        public override void OnDown(int cursorId, Vector2 position, CursorKey cursorKey)
        {
            IsPressed = true;
            base.OnDown(cursorId, position, cursorKey);
        }

        public override void OnUp(int cursorId, Vector2 position, CursorKey cursorKey)
        {
            IsPressed = false;
            base.OnUp(cursorId, position, cursorKey);
        }

        #endregion Public Methods
    }
}