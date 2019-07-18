using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using OpenTK.Input;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Components
{
    public class KeyboardControl : IEntityComponent
    {
        #region Public Constructors

        public KeyboardControl(Key moveUpKey, Key moveDownKey, Key moveLeftKey, Key moveRightKey)
        {
            MoveUpKey = moveUpKey;
            MoveDownKey = moveDownKey;
            MoveLeftKey = moveLeftKey;
            MoveRightKey = moveRightKey;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Direction { get; set; }
        public Key MoveUpKey { get; set; }
        public Key MoveDownKey { get; set; }
        public Key MoveLeftKey { get; set; }
        public Key MoveRightKey { get; set; }

        #endregion Public Properties
    }
}