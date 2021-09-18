﻿using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Commands;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class StaticBodiesSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private const int CELL_SIZE = 16;

        private readonly List<int> inactiveStatics = new List<int>();
        private readonly IEntityMan entityMan;
        private readonly IFixtureMan fixtureMan;
        private IBroadphaseGrid broadphaseGrid;

        #endregion Private Fields

        #region Internal Constructors

        internal StaticBodiesSystem(IEntityMan entityMan, IFixtureMan fixtureMan)
        {
            this.entityMan = entityMan;
            this.fixtureMan = fixtureMan;

            RequireEntityWith<BodyComponent>();
            RequireEntityWith<PositionComponent>();
            RequireEntityWithout<VelocityComponent>();

            RegisterHandler<BodyOnCommand>(HandleBodyOnCommand);
            RegisterHandler<BodyOffCommand>(HandleBodyOffCommand);
        }

        #endregion Internal Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            broadphaseGrid = world.GetModule<IBroadphaseGrid>();
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            ExecuteCommands();
        }

        public void Update(float dt)
        {
            ExecuteCommands();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            InsertToGrid(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            RemoveFromGrid(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private static Box2 GetAabb(Entity entity)
        {
            var body = entity.Get<BodyComponent>();
            return body.Aabb;
        }

        private bool HandleBodyOnCommand(BodyOnCommand cmd)
        {
            var entity = entityMan.GetById(cmd.EntityId);

            if (inactiveStatics.Contains(cmd.EntityId))
            {
                InsertToGrid(entity);
                inactiveStatics.Remove(cmd.EntityId);
                entity.RaiseEvent(new BodyOnEventArgs(entity));
                return true;
            }

            return false;
        }

        private bool HandleBodyOffCommand(BodyOffCommand cmd)
        {
            var entity = entityMan.GetById(cmd.EntityId);

            RemoveFromGrid(entity);

            inactiveStatics.Add(entity.Id);
            entity.RaiseEvent(new BodyOffEventArgs(entity));
            return true;
        }

        private void UpdateAabb(BodyComponent body, PositionComponent pos)
        {
            var fixture = fixtureMan.GetById(body.Fixtures.First());
            body.Aabb = fixture.Shape.GetAabb().Translated(pos.Value);
        }

        private void InsertToGrid(Entity entity)
        {
            var pos = entity.Get<PositionComponent>();
            var body = entity.Get<BodyComponent>();

            UpdateAabb(body, pos);

            broadphaseGrid.InsertStatic(entity.Id, body.Aabb);
        }

        private void RemoveFromGrid(Entity entity)
        {
            var aabb = GetAabb(entity);
            broadphaseGrid.RemoveStatic(entity.Id, aabb);
        }

        #endregion Private Methods
    }
}