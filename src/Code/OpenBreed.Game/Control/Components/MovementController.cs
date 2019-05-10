using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Movement.Components;
using OpenTK.Input;
using System;
using System.Linq;

namespace OpenBreed.Game.Control.Components
{
    public class MovementController : IControlComponent
    {
        #region Private Fields

        private CreatureMovement movement;

        #endregion Private Fields

        #region Public Constructors

        public MovementController()
        {
        }

        #endregion Public Constructors

        #region Public Properties

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
            if (keyState[Key.Up])
                movement.Move(MovementDirection.Up);
            else if (keyState[Key.Down])
                movement.Move(MovementDirection.Down);

            if (keyState[Key.Left])
                movement.Move(MovementDirection.Left);
            else if (keyState[Key.Right])
                movement.Move(MovementDirection.Right);


        }

        #endregion Public Methods
    }
}