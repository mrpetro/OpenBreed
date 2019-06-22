using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Core.Systems.Movement;
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
    public class AiControlSystem : WorldSystemEx, IUpdatableSystemEx
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();

        #endregion Private Fields

        #region Public Constructors

        public AiControlSystem(ICore core) : base(core)
        {
            Require<AiControl>();
            Require<Position>();
            Require<StateMachine>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                ControlEntity(dt, entities[i]);
        }

        private void ControlEntity(float dt, IEntity entity)
        {
            var control = entity.Components.OfType<AiControl>().First();
            var position = entity.Components.OfType<Position>().First();
            var stateMachine = entity.Components.OfType<StateMachine>().First();

            if (control.Waypoints.Any())
            {
                var distanceVec = Vector2.Subtract(control.Waypoints.First(), position.Value);
                var targetVector = distanceVec;

                targetVector.Normalize();

                int compass = (((int)Math.Round(Math.Atan2(targetVector.Y, targetVector.X) / (2 * Math.PI / 8))) + 8) % 8;

                //Console.WriteLine($"Distance Left: {targetVector.Length}");

                //Console.WriteLine($"Move Dir: {targetVector}");

                if (distanceVec.Length < 16.0f)
                {
                    //Force position of entity when it's close enough to waypoint
                    position.Value = control.Waypoints.First();
                    control.Waypoints.RemoveAt(0);
                    return;
                }

                var direction = MovementTools.SnapToCompass8Way(targetVector);
                stateMachine.Perform("Walk", direction);
            }
            else
                stateMachine.Perform("Stop");
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