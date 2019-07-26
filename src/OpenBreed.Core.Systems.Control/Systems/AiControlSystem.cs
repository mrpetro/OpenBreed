using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Common.Systems.Helpers;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Components;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Systems
{
    /// <summary>
    /// Control system should be used to control actions
    /// </summary>
    public class AiControlSystem : WorldSystem, IUpdatableSystem
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

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity);
            aiControlComps.Add(entity.Components.OfType<AiControl>().First());
            positionComps.Add(entity.Components.OfType<IPosition>().First());
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var index = entities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
            aiControlComps.RemoveAt(index);
            positionComps.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private void ControlEntity(float dt, int index)
        {
            var entity = entities[index];
            var control = aiControlComps[index];
            var position = positionComps[index];

            var direction = Vector2.Zero;

            if (control.Waypoints.Any())
            {
                var distanceVec = Vector2.Subtract(control.Waypoints.First(), position.Value);
                var targetVector = distanceVec;

                targetVector.Normalize();

                int compass = (((int)Math.Round(Math.Atan2(targetVector.Y, targetVector.X) / (2 * Math.PI / 8))) + 8) % 8;

                if (distanceVec.Length < 16.0f)
                {
                    //Force position of entity when it's close enough to waypoint
                    position.Value = control.Waypoints.First();
                    control.Waypoints.RemoveAt(0);
                    return;
                }

                direction = MovementTools.SnapToCompass8Way(targetVector);
            }

            if (control.Direction == direction)
                return;

            control.Direction = direction;

            entity.RaiseEvent(new ControlDirectionChangedEvent(control.Direction));
        }

        #endregion Private Methods
    }
}