using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Control;
using OpenBreed.Core.Systems.Control.Components;
using OpenTK;
using OpenTK.Input;
using System;
using System.Linq;

namespace OpenBreed.Game.Components
{
    public class KeyboardCreatureController : IKeyboardController
    {
        #region Private Fields

        private CreatureMovement movement;

        #endregion Private Fields

        #region Public Constructors

        public KeyboardCreatureController(Key moveUpKey, Key moveDownKey, Key moveLeftKey, Key moveRightKey)
        {
            MoveUpKey = moveUpKey;
            MoveDownKey = moveDownKey;
            MoveLeftKey = moveLeftKey;
            MoveRightKey = moveRightKey;
        }

        #endregion Public Constructors

        #region Public Properties

        public Key MoveUpKey { get; set; }
        public Key MoveDownKey { get; set; }
        public Key MoveLeftKey { get; set; }
        public Key MoveRightKey { get; set; }
        public Type SystemType { get { return typeof(ControlSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IEntity entity)
        {
            movement = entity.Components.OfType<CreatureMovement>().First();
        }

        public void ProcessInputs(float dt, KeyboardState keyState)
        {
            var direction = new Vector2(0, 0);

            if (keyState[MoveLeftKey])
                direction.X = -1;
            else if (keyState[MoveRightKey])
                direction.X = 1;

            if (keyState[MoveUpKey])
                direction.Y = 1;
            else if (keyState[MoveDownKey])
                direction.Y = -1;

            movement.Move(direction);
        }

        #endregion Public Methods
    }
}