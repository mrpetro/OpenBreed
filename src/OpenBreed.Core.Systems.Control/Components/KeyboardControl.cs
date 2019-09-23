using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using OpenTK.Input;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Components
{
    public class KeyboardControl : IEntityComponent
    {
        #region Public Constructors

        public KeyboardControl(Key moveUpKey, Key moveDownKey, Key moveLeftKey, Key moveRightKey, Key fireKey)
        {
            MoveUpKey = moveUpKey;
            MoveDownKey = moveDownKey;
            MoveLeftKey = moveLeftKey;
            MoveRightKey = moveRightKey;
            FireKey = fireKey;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Direction { get; set; }
        public bool Fire { get; set; }
        public Key MoveUpKey { get; set; }
        public Key MoveDownKey { get; set; }
        public Key MoveLeftKey { get; set; }
        public Key MoveRightKey { get; set; }

        public Key FireKey { get; set; }

        #endregion Public Properties
    }
}