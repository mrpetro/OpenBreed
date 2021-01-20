using OpenBreed.Core;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Systems;
using OpenBreed.Systems.Core;
using OpenBreed.Systems.Physics.Events;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Systems.Physics
{
    public class DirectionSystem : WorldSystem, IUpdatableSystem
    {
        private const float FLOOR_FRICTION = 1.0f;

        #region Private Fields

        private readonly List<int> entities = new List<int>();

        #endregion Private Fields

        #region Public Constructors

        public DirectionSystem(ICore core) : base(core)
        {
            Require<AngularPositionComponent>();
            Require<AngularVelocityComponent>();
            Require<AngularThrustComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void UpdatePauseImmuneOnly(float dt)
        {
        }

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                UpdateEntity(dt, entities[i]);
        }

        public void UpdateEntity(float dt, int id)
        {
            var entity = Core.Entities.GetById(id);
            var angularPos = entity.Get<AngularPositionComponent>();
            var angularVel = entity.Get<AngularVelocityComponent>();
            var angularThrust = entity.Get<AngularThrustComponent>();

            //Velocity equation
            //var newVel = angularVel.Value + angularThrust.Value * dt;

            var aPos = angularPos.GetDirection();
            var dPos = angularVel.GetDirection();

            var newPos = aPos.RotateTowards(dPos, (float)Math.PI * 0.125f, 1.0f);
            //newPos = MovementTools.SnapToCompass8Way(newPos);

            if (newPos == aPos)
                return;

            angularPos.SetDirection(newPos);
            entity.RaiseEvent(new DirectionChangedEventArgs(angularPos.GetDirection()));
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity.Id);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            var index = entities.IndexOf(entity.Id);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
        }

        #endregion Protected Methods
    }
}
