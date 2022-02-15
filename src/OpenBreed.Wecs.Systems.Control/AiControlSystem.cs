using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Control
{
    /// <summary>
    /// Control system should be used to control actions
    /// </summary>
    public class AiControlSystem : UpdatableSystemBase
    {
        #region Internal Constructors

        internal AiControlSystem()
        {
            RequireEntityWith<AiControl>();
            RequireEntityWith<PositionComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(Entity entity, IWorldContext context)
        {
            var control = entity.Get<AiControl>();
            var position = entity.Get<PositionComponent>();

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

            entity.RaiseEvent(new ControlDirectionChangedEventArgs(control.Direction));
        }

        #endregion Protected Methods
    }
}