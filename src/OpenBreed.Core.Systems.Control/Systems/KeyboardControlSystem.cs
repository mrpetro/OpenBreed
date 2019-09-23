﻿using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Components;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Systems
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

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var index = entities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private void ProcessInputs(float dt, KeyboardState keyState, IEntity entity)
        {
            var direction = Vector2.Zero;
            var fire = false;


            var control = entity.Components.OfType<KeyboardControl>().First();

            if (keyState[control.FireKey])
                fire = true;

            if (keyState[control.MoveLeftKey])
                direction.X = -1;
            else if (keyState[control.MoveRightKey])
                direction.X = 1;

            if (keyState[control.MoveUpKey])
                direction.Y = 1;
            else if (keyState[control.MoveDownKey])
                direction.Y = -1;

            if (control.Direction != direction)
            {
                control.Direction = direction;
                entity.RaiseEvent(new ControlDirectionChangedEvent(control.Direction));
            }

            if (control.Fire != fire)
            {
                control.Fire = fire;
                entity.RaiseEvent(new ControlFireChangedEvent(control.Fire));
            }
        }

        #endregion Private Methods
    }
}