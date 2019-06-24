using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Core.Systems.Movement;
using OpenTK;
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
        private readonly List<AiControl> aiControlComps = new List<AiControl>();
        private readonly List<IPosition> positionComps = new List<IPosition>();

        #endregion Private Fields

        #region Public Constructors

        public AiControlSystem(ICore core) : base(core)
        {
            Require<AiControl>();
            Require<IPosition>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                ControlEntity(dt, i);
        }

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
            aiControlComps.Add(entity.Components.OfType<AiControl>().First());
            positionComps.Add(entity.Components.OfType<IPosition>().First());
        }

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Public Methods

        #region Private Methods

        private void ControlEntity(float dt, int index)
        {
            var entity = entities[index];
            var control = aiControlComps[index];
            var position = positionComps[index];

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
                entity.PerformDelegate("Walk", direction);
            }
            else
                entity.PerformDelegate("Stop");
        }

        #endregion Private Methods
    }
}