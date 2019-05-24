using OpenTK;
using OpenTK.Input;
using System;

namespace OpenBreed.Core
{
    public class InputsMan
    {
        #region Public Constructors

        public InputsMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler<KeyboardKeyEventArgs> KeyDown;

        public event EventHandler<KeyboardKeyEventArgs> KeyUp;

        public event EventHandler<KeyPressEventArgs> KeyPress;

        #endregion Public Events

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public void OnKeyDown(KeyboardKeyEventArgs e)
        {
            KeyDown?.Invoke(this, e);
        }

        public void OnKeyUp(KeyboardKeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);
        }

        public void OnKeyPress(KeyPressEventArgs e)
        {
            KeyPress?.Invoke(this, e);
        }

        #endregion Public Methods
    }
}