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
using OpenBreed.Rendering.Abstractions.Events;
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