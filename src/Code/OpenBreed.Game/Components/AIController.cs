using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Control;
using OpenBreed.Core.Systems.Control.Components;
using OpenTK.Input;
using System;
using System.Linq;

namespace OpenBreed.Game.Components
{
    public class AIController : IControllerComponent
    {
        #region Private Fields

        private CreatureMovement movement;

        #endregion Private Fields

        #region Public Constructors

        public AIController()
        {
            //MoveUpKey = moveUpKey;
            //MoveDownKey = moveDownKey;
            //MoveLeftKey = moveLeftKey;
            //MoveRightKey = moveRightKey;
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

        public void ProcessInputs(KeyboardState keyState)
        {
            if (keyState[MoveUpKey])
                movement.Move(MovementDirection.Up);
            else if (keyState[MoveDownKey])
                movement.Move(MovementDirection.Down);

            if (keyState[MoveLeftKey])
                movement.Move(MovementDirection.Left);
            else if (keyState[MoveRightKey])
                movement.Move(MovementDirection.Right);
        }

        #endregion Public Methods
    }
}