using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Core.Systems.Control.Events;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems.Control.Systems
{
    /// <summary>
    /// Control system should be used to control actions
    /// </summary>
    public class KeyboardControlSystem : WorldSystem, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();

        #endregion Private Fields

        #region Public Constructors

        public KeyboardControlSystem(ICore core) : base(core)
        {  
            Require<KeyboardControl>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(float dt)
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetCursorState();

            for (int i = 0; i < entities.Count; i++)
                ProcessInputs(dt, keyState, entities[i]);

        }

        private void ProcessInputs(float dt, KeyboardState keyState, IEntity entity)
        {
            var direction = Vector2.Zero;

            var control = entity.Components.OfType<KeyboardControl>().First();

            if (keyState[control.MoveLeftKey])
                direction.X = -1;
            else if (keyState[control.MoveRightKey])
                direction.X = 1;

            if (keyState[control.MoveUpKey])
                direction.Y = 1;
            else if (keyState[control.MoveDownKey])
                direction.Y = -1;

            if (control.Direction == direction)
                return;

            control.Direction = direction;
            entity.HandleSystemEvent?.Invoke(this, new ControlDirectionChangedEvent(control.Direction));
        }

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Public Methods


    }
}