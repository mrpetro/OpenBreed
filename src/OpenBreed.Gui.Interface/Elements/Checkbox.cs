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
    internal class Checkbox : Element, ICheckbox
    {
        #region Internal Constructors

        internal Checkbox(CheckboxBuilder builder) : base(builder)
        {
            IsChecked = builder.IsChecked;
        }

        #endregion Internal Constructors

        #region Public Properties

        public bool IsPressed { get; private set; }

        public bool IsChecked { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public override void OnClick(int cursorId, CursorKey cursorKey)
        {
            IsChecked = !IsChecked;

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